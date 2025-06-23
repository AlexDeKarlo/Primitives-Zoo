using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public AnimalFabric AnimalFabric;
    
    public override void InstallBindings()
    {
        Container.Bind<Frog>().FromNewComponentOnNewGameObject().AsTransient();
        Container.Bind<Snake>().FromNewComponentOnNewGameObject().AsTransient();

        Container.Bind<IMovement>()
                 .To<JumpMovement>()
                 .AsTransient()
                 .WhenInjectedInto<Frog>();

        Container.Bind<IMovement>()
                 .To<SlitherMovement>()
                 .AsTransient()
                 .WhenInjectedInto<Snake>();

        Container.Bind<IHunt>()
                 .To<PredatorHunt>()
                 .AsTransient();

        Container.Bind<IConflict>()
                 .To<RandomConflict>()
                 .AsTransient();

        Container.Bind<IKnockback>()
                 .To<OpposingKnockbackMovement>()
                 .AsTransient();
        
        Container.Bind<IAnimalFabric>()
            .To<AnimalFabric>()
            .FromInstance(AnimalFabric)
            .AsTransient();
    }
    
}
