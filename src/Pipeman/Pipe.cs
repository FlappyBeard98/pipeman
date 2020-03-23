using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Pipeman
{
    public class Pipe<TDone,TNotDone> : IPipe<TDone,TNotDone>
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly List<( Type phase ,bool isFinal)> _phasesOrder;
        private readonly List<( Type item ,bool refresh)> _items;
        
        internal IEnumerable<Type> Phases => _phasesOrder.Select(p => p.phase);
        internal IEnumerable<Type> Items => _items.Select(p => p.item);
        
        
        public Pipe()
        {
            _serviceCollection = new ServiceCollection();
            _phasesOrder=new List<(Type , bool )>();
            _items = new List<(Type , bool )>();
        }

        //todo объединитьрегистрации айтемов
        private void RegisterItem(Type serviceType,Type implType,object item=default,bool useItem=false,bool refresh=false) 
        {
            var isDefault = item == default;
            var type = isDefault || useItem ? serviceType : item.GetType();

            var oldService = _serviceCollection.FirstOrDefault(p => p.ServiceType == type);

            if (oldService != null)
            {
                _serviceCollection.Remove(oldService);
                _items.Remove(_items.FirstOrDefault(p => p.item == type));
            }
            
            if (isDefault)
                _serviceCollection.AddSingleton(serviceType,implType);
            else
                _serviceCollection.AddSingleton(type,p=>item);
            
            _items.Add((type,refresh));
            
        }
        private void RegisterItem<TItem,TImplementation>(TImplementation item=default,bool useItem=false,bool refresh=false) where TItem : class where TImplementation : class, TItem
        {
            var isDefault = item == default;
            var type = isDefault || useItem ? typeof(TItem) : item.GetType();

            var oldService = _serviceCollection.FirstOrDefault(p => p.ServiceType == type);

            if (oldService != null)
            {
                _serviceCollection.Remove(oldService);
                _items.Remove(_items.FirstOrDefault(p => p.item == type));
            }
            
            if (isDefault)
                _serviceCollection.AddSingleton<TItem,TImplementation>();
            else
                _serviceCollection.AddSingleton(type,p=>item);
            
            _items.Add((type,refresh));
            
        }

        public IPipe<TDone,TNotDone> Pass<TItem>(TItem item=default,bool refresh=false) where TItem : class
        {
            RegisterItem<TItem,TItem>(item,refresh);
            return this;
        }
        
        public IPipe<TDone,TNotDone> Pass<TItem,TImplementation>(TImplementation item=default,bool refresh=false) where TItem : class where TImplementation : class, TItem
        {
            RegisterItem<TItem,TImplementation>(item,true,refresh);
            return this;
        }

        private void RegisterPhase<TPhase>(bool isFinal,TPhase phase = default) where TPhase : class, IPhase
        {
            if (_phasesOrder.Select(p => p.phase).Contains(typeof(TPhase)))
            {
                _phasesOrder.Add((typeof(TPhase),isFinal));
                return;
            }
            
            if (phase == default)
                _serviceCollection.AddSingleton<TPhase>();
            else
                _serviceCollection.AddSingleton(p=>phase);
            
            _phasesOrder.Add((typeof(TPhase),isFinal));
        }
        
        public IPipe<TDone,TNotDone> Next<TPhase>(TPhase phase=default) where TPhase : class,IPhase
        {
            RegisterPhase(false,phase);
            return this;
        }

        public IPipe<TDone, TNotDone> CompleteWhenDone<TPhase>(TPhase phase = default) where TPhase : class,IPhaseDone<TDone> 
        {
            RegisterPhase(true,phase);
            return this;
        }

        public IPipe<TDone, TNotDone> CompleteWhenNotDone<TPhase>(TPhase phase = default) where TPhase : class,IPhaseNotDone<TNotDone> 
        {
            RegisterPhase(true,phase);
            return this;
        }

        public IPipe<TDone, TNotDone> Complete<TPhase>(TPhase phase = default) where TPhase : class,IPhase<TDone, TNotDone>
        {
            RegisterPhase(true,phase);
            return this;
        }


        private void RefreshItems(ServiceProvider sp)
        {
            var refreshing = _items.Where(p => p.refresh).Select(p => p.item).ToList();
            foreach (var type in refreshing)
            {
                var item =  sp.GetRequiredService(type);
                var service = _serviceCollection.First(p => p.ServiceType == type);
                RegisterItem(service.ServiceType,item.GetType(),item,useItem:true,refresh:true);
            }
        }
        
        public PipeResult<TDone, TNotDone> Execute()
        {

            foreach (var (type, isFinal) in _phasesOrder)
                using (var sp = _serviceCollection.BuildServiceProvider())
                {
                    var phase = (IPhase) sp.GetRequiredService(type);
                    var phaseResult = phase.Execute();
                    RegisterItem<object,object>(phaseResult.GetPayload());
                    RefreshItems(sp);
                    switch (phase)
                    {
                        case IPhase<TDone, TNotDone> _ when isFinal:
                            return phaseResult.FromPhaseResult<TDone, TNotDone>();
                        case IPhaseDone<TDone> _ when isFinal && phaseResult is DoneResult<TDone> d:
                            return PipeResult<TDone, TNotDone>.Done(d.Payload);
                        case IPhaseNotDone<TNotDone> _ when isFinal && phaseResult is NotDoneResult<TNotDone> d:
                            return PipeResult<TDone, TNotDone>.NotDone(d.Payload);

                    }
                }

            throw new InvalidOperationException("there are no any results in pipe");
        }
    }
}