using System;
using System.Collections.Generic;
using System.Text;

namespace BTNFramework.BTN  
{
    public sealed class EntityManager
    { 
        private IDictionary<int, Aspect> _entityMasks;

        private IDictionary<int, Dictionary<Type, IComponent>> _assignedComponents;

        private IList<int> _availableIds;

        private int _nextId;

        private IList<int> _activeEntities;

        private IList<Tuple<int, IComponent>> _componentsToRemove;

        private EntityWorld _world;

        public EntityManager(EntityWorld world)
        {
            _world = world;
            _nextId = 0;

            _componentsToRemove = new List<Tuple<int, IComponent>>();
            _assignedComponents = new Dictionary<int, Dictionary<Type, IComponent>>();
            _availableIds = new List<int>();
            _activeEntities = new List<int>();
            _entityMasks = new Dictionary<int, Aspect>();
        }

        public int CreateEntity()
        {
            int id;
            if (_availableIds.Count > 0) {
                id = _availableIds[0];
                _availableIds.RemoveAt(0);
            } else {
                id = _nextId;
                _nextId++;
            }

            _entityMasks.Add(id, new Aspect());
            _assignedComponents.Add(id, new Dictionary<Type, IComponent>());
            _activeEntities.Add(id);

            return id;
        }

        public int CreateEntity(params IComponent[] components)
        {
            int entity = CreateEntity();

            foreach (IComponent component in components) {
                AddComponent(entity, component.GetType(), component);
            }

            return entity;
        }

        public Aspect GetAspect(int id)
        {
            if (_entityMasks.ContainsKey(id)) {
                return _entityMasks[id];
            }
            return null;
        }

        public T GetComponent<T>(int id) where T : IComponent
        {
            if (_assignedComponents.ContainsKey(id) && _assignedComponents[id].ContainsKey(typeof(T))) {
                return (T)(_assignedComponents[id][typeof(T)]);
            }
            return default(T);
        }

        public void RemoveComponent<T>(int id) where T : IComponent
        {
            if (_assignedComponents.ContainsKey(id) && _assignedComponents[id].ContainsKey(typeof(T))) {
                _componentsToRemove.Add(new Tuple<int, IComponent>(id, _assignedComponents[id][typeof(T)]));
                _assignedComponents[id].Remove(typeof(T));
            }
        }

        public void AddComponent<T>(int id, T component) where T : IComponent
        {
            if (_assignedComponents.ContainsKey(id) && !_assignedComponents[id].ContainsKey(typeof(T))) {
                _assignedComponents[id].Add(typeof(T), component);
                _entityMasks[id].Mask += ComponentTypeManager.GetBitFor<T>();
            }
        }

        private void AddComponent(int id, Type t, IComponent comp)
        {
            if (_assignedComponents.ContainsKey(id) && !_assignedComponents[id].ContainsKey(t)) {
                _assignedComponents[id].Add(t, comp);
                _entityMasks[id].Mask += ComponentTypeManager.GetBitFor(t);
            }
        }

        public void RemoveEntity(int entity)
        {
            if (_activeEntities.Contains(entity)) {
                foreach(KeyValuePair<Type, IComponent> compPair in _assignedComponents[entity]) {
                    _componentsToRemove.Add(new Tuple<int, IComponent>(entity, compPair.Value));
                }

                _activeEntities.Remove(entity);
                _entityMasks.Remove(entity);
                _assignedComponents.Remove(entity);

                _availableIds.Add(entity);
            }
        }

        private void RemoveAllComponents(int entity)
        {
            if (!_activeEntities.Contains(entity)) { return; }

            foreach(KeyValuePair<Type, IComponent> compPair in _assignedComponents[entity]) {
                _entityMasks[entity].Mask -= ComponentTypeManager.GetBitFor(compPair.Key);
            }
            _world.UpdateEntity(entity, _entityMasks[entity]);

            _assignedComponents[entity].Clear();
            
        }

        public void CleanComponents()
        {
            foreach (Tuple<int, IComponent> compTuple in _componentsToRemove) {
                if (_activeEntities.Contains(compTuple.Item1)) {
                    _entityMasks[compTuple.Item1].Mask -= ComponentTypeManager.GetBitFor(compTuple.Item2.GetType());
                    _world.UpdateEntity(compTuple.Item1, _entityMasks[compTuple.Item1]);
                }
            }

            _componentsToRemove.Clear();
        }
    }
}
