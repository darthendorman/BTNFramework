using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTNFramework.BTN
{
    public class EntityWorld
    {
        public EntityManager EntityMgr;
        public ProcessorManager PrcsMgr;

        public EntityWorld()
        {
            EntityMgr = new EntityManager(this);
            PrcsMgr = new ProcessorManager(this);
        }

        public void UpdateEntity(int entity, Aspect entityAspect)
        {
            PrcsMgr.UpdateInterest(entity, entityAspect);
        }

        public void AddComponent<T>(int entity, T component) where T:IComponent
        {
            EntityMgr.AddComponent<T>(entity, component);
            PrcsMgr.UpdateInterest(entity, EntityMgr.GetAspect(entity));
        }

        public T GetComponent<T>(int entity) where T : IComponent
        {
            return EntityMgr.GetComponent<T>(entity);
        }

        public void RemoveComponent<T>(int entity) where T : IComponent
        {
            EntityMgr.RemoveComponent<T>(entity);
            PrcsMgr.UpdateInterest(entity, EntityMgr.GetAspect(entity));
        }

        public int CreateEntity()
        {
            int id = EntityMgr.CreateEntity();
            PrcsMgr.UpdateInterest(id, EntityMgr.GetAspect(id));
            return id;
        }

        public int CreateEntity(params IComponent[] components)
        {
            int id = EntityMgr.CreateEntity(components);
            PrcsMgr.UpdateInterest(id, EntityMgr.GetAspect(id));
            return id;
        }

        public void RemoveEntity(int entity)
        {
            PrcsMgr.RemoveEntity(entity);
            EntityMgr.RemoveEntity(entity);
        }

        public void AddProcessor(Processor process, bool updatable)
        {
            PrcsMgr.AddProcessor(process, updatable);
        }

        public void RemoveProcessor<T>() where T : Processor
        {
            PrcsMgr.RemoveProcessor<T>();
        }

        public void Update(float dt)
        {
            EntityMgr.CleanComponents();
            PrcsMgr.CleanEntities();
            PrcsMgr.CleanProcessors();
            PrcsMgr.Update(dt);
        }

        public void Render(SpriteBatch batch)
        {
            PrcsMgr.Render(batch);
        }
    }
}
