namespace RedstoneByte.Utils
{
    public enum EntityStatus
    {
        // The base class.
        #region Entity

        EntityGuardianSoundEffect = 21,
        EntityTotemOfUndyingAnimation = 35,

        #endregion

        // Extends Entity.
        #region Fishing Hook

        FishingHookPullPlayer = 31,

        #endregion

        // Extends Entity.
        #region Tipped Arrow

        TippedArrowSpawnParticleEffects = 0,

        #endregion

        // Extends Entity.
        #region Fireworks

        FireworksTriggerExplosionEffect = 17,

        #endregion

        // Extends Entity.
        #region Living

        LivingHurtAnimation = 2,
        LivingDeathAnimation = 3,
        LivingShieldBlockSound = 29,
        LivingShieldBreakSound = 30,
        LivingThornsSound = 33,

        #endregion

        // Extends Living.
        #region Player

        PlayerFinishUse = 9,
        PlayerReducedDebugScreen = 22,
        PlayerEnhanceDebugScreen = 23,
        PlayerOp0 = 24,
        PlayerOp1 = 25,
        PlayerOp2 = 26,
        PlayerOp3 = 27,
        PlayerOp4 = 28,

        #endregion

        // Extends Living.
        #region Armor Stand

        ArmorStandHit = 32,

        #endregion

        // Extends Living.
        #region Insentient

        InsentientExplosionParticle = 20,

        #endregion

        // Extends Insentient.
        #region Squid

        SquidResetRotation = 19,

        #endregion

        // Extends Insentient.
        #region Animal

        AnimalHeartParticles = 18,

        #endregion

        // Extends Animal.
        #region Horse and Tameable Animal

        TamingFailed = 6,
        TamingSucceeded = 7,

        #endregion

        // Extends Animal.
        #region Rabbit

        RabbitJumpingAnimation = 1,

        #endregion

        // Extends Animal.
        #region Sheep

        SheepEatingGrassAnimation = 10,

        #endregion

        // Extends Tameable Animal.
        #region Wolf

        WolfShakingWaterAnimation = 8,

        #endregion

        // Extends Insentient.
        #region Villager

        VillagerMatingHeartParticles = 12,
        VillagerAngryParticles = 13,
        VillagerHappyParticles = 14,

        #endregion

        // Extends Insentient.
        #region Iron Golem

        IronGolemAttackAnimation = 4,
        IronGolemHoldPoppy = 11,
        IronGolemPutAawayPoppy = 34,

        #endregion

        // Extends Insentient.
        #region Zombie Villager

        ZombieVillagerCureFinishedSound = 16,

        #endregion

        // Extends Entity.
        #region Minecart TNT

        MinecartTntIgnite = 10,

        #endregion

        // Extends Entity.
        #region Minecart Spawner

        MinecartSpawnerResetDelay = 1

        #endregion
    }
}