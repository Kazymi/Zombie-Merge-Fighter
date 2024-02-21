using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectSpawner : MonoBehaviour, IEffectSpawner
{
    [SerializeField] private List<EffectConfiguration> effectConfigurations;
 
     private Dictionary<EffectType, IPool<TemporaryMonoPooled>> effects =
         new Dictionary<EffectType, IPool<TemporaryMonoPooled>>();
 
     private void OnEnable()
     {
         ServiceLocator.Subscribe<IEffectSpawner>(this);
     }
 
     private void OnDisable()
     {
         ServiceLocator.Unsubscribe<IEffectSpawner>();
     }
 
     public void SpawnEffect(EffectType effectType, Transform position, bool customRotation = true)
     {
         TryToInitializeEffect(effectType);
         var newEffect = effects[effectType].Pull();
         newEffect.transform.position = position.position;
         if (customRotation)
         {
             newEffect.transform.rotation = position.rotation;
         }
     }
 
     private void TryToInitializeEffect(EffectType effectType)
     {
         if (effects.ContainsKey(effectType))
         {
             return;
         }
 
         var effect = effectConfigurations.Where(t => t.EffectType == effectType).ToList();
         if (effect.Count == 0)
         {
             Debug.LogError($"Effect {effectType} not initialized");
             return;
         }
 
         var factoryMono = new FactoryMonoObject<TemporaryMonoPooled>(effect[0].Effect.gameObject, transform);
         effects.Add(effectType, new Pool<TemporaryMonoPooled>(factoryMono, 1));
     }
 }

public interface IEffectSpawner
{
    void SpawnEffect(EffectType effectType, Transform position, bool customRotation = true);
}