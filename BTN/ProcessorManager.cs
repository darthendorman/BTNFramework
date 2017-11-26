using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTNFramework.BTN
{
    public class ProcessorManager
    {
        private EntityWorld _world;

        private List<Processor> _processors;

        private List<Processor> _updateProcessors;

        private List<Processor> _renderProcessors;

        private List<Processor> _processorsToRemove;

        public ProcessorManager(EntityWorld world)
        {
            _world = world;
            _processors = new List<Processor>();
            _updateProcessors = new List<Processor>();
            _renderProcessors = new List<Processor>();
            _processorsToRemove = new List<Processor>();
        }

        public void Update(float dt)
        {
            foreach (var process in _updateProcessors) {
                process.Update(dt);
            }
        }

        public void Render(SpriteBatch batch)
        {
            foreach (var process in _renderProcessors) {
                process.Render(batch);
            }
        }

        public void AddProcessor(Processor process, bool updatable)
        {
            if (updatable) {
                _updateProcessors.Add(process);
            } else {
                _renderProcessors.Add(process);
            }
            _processors.Add(process);
        }

        public void RemoveProcessor<T>() where T : Processor
        {
            _processorsToRemove = _processors.Where(c => c.GetType() == typeof(T)).ToList();
        }

        public void CleanProcessors()
        {
            foreach (Processor process in _processorsToRemove) {
                if (_updateProcessors.Contains(process)) {
                    _updateProcessors.Remove(process);
                }
                if (_renderProcessors.Contains(process)) {
                    _renderProcessors.Remove(process);
                }
                _processors.Remove(process);
            }
            _processorsToRemove.Clear();
        }

        public void RemoveEntity(int entity)
        {
            foreach (Processor process in _processors) {
                process.RemoveEntity(entity);
            }
        }

        public void CleanEntities ()
        {
            foreach (Processor process in _processors) {
                process.CleanUpRemoval();
            }
        }

        public void UpdateInterest(int entity, Aspect aspect)
        {
            foreach (Processor process in _processors) {
                process.UpdateInterest(entity, aspect);
            }
        }

        public EntityWorld GetWorld()
        {
            return _world;
        }
    }
}
