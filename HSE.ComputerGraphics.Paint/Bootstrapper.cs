using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using HSE.ComputerGraphics.Paint.ViewModels;
using System.Windows.Input;

namespace HSE.ComputerGraphics.Paint
{
    public class Bootstrapper : BootstrapperBase
    {
        //private CompositionContainer container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            //container = CompositionHost.Initialize(
            //    new AggregateCatalog(
            //        AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
            //    )
            //);

            //var batch = new CompositionBatch();

            //batch.AddExportedValue<IWindowManager>(new WindowManager());
            //batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            //batch.AddExportedValue(container);
            // pizdec
            //container.Compose(batch);

            MessageBinder.SpecialValues.Add("$mousepoint", ctx =>
            {
                var e = ctx.EventArgs as MouseEventArgs;
                if (e == null)
                {
                    return null;
                }

                return e.GetPosition(ctx.Source);
            });
        }
        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<AppViewModel>();
        }
    }
}
