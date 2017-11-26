using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTNFramework.BTN
{
    public abstract class Processor
    {
        protected IList<int> _entities;
        private IList<int> _entitiesToRemove;
        protected Aspect _inclAspect;
        protected Aspect _exclAspect;

        protected ProcessorManager _prcsMgr;

        public Processor(ProcessorManager mgr, Aspect inclAspect = null, Aspect exclAspect = null)
        {
            _prcsMgr = mgr;
            _entities = new List<int>();
            _entitiesToRemove = new List<int>();
            _inclAspect = inclAspect;
            _exclAspect = exclAspect;
        }

        public void AddEntity(int entity, Aspect aspect)
        {
            if (_entities.Contains(entity)) { return; }

            if(_inclAspect != null && _inclAspect.Intersects(aspect)) {
                if (_exclAspect == null || !_exclAspect.Intersects(aspect)) {
                    _entities.Add(entity);
                }
            }

        }

        public void UpdateInterest(int entity, Aspect aspect)
        {
            if (_entities.Contains(entity)) {
                if ((_inclAspect != null && !_inclAspect.Intersects(aspect)) || 
                    (_exclAspect != null && _exclAspect.Intersects(aspect))) {
                    _entities.Remove(entity);
                }
            } else {
                if (_inclAspect != null && _inclAspect.Intersects(aspect)) {
                    if(_exclAspect == null || !_exclAspect.Intersects(aspect)) {
                        _entities.Add(entity);
                    }
                }
            }
        }

        public void RemoveEntity(int entity)
        {
            if (_entities.Contains(entity)) {
                _entitiesToRemove.Add(entity);
                
            }
        }

        public void CleanUpRemoval()
        {
            foreach (int entity in _entitiesToRemove) {
                _entities.Remove(entity);
            }
            _entitiesToRemove.Clear();
        }

        public virtual void Update(float dt) { }
        public virtual void Render(SpriteBatch batch) { }
    
        protected EntityWorld World
        {
            get {
                return _prcsMgr.GetWorld();
            }
        }
    }
}
