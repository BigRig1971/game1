namespace StupidHumanGames {
[System.Serializable]
public sealed class CreatureMountZSerializer : ZSerializer.Internal.ZSerializer
{
    public System.Single RotationSmoothTime;
    public UnityEngine.Transform _mountTransform;
    public UnityEngine.AudioClip[] _randomAttackSound;
    public System.Single _randomAttackSoundVolume;
    public UnityEngine.AudioClip[] _randomSound;
    public System.Single _randomSoundVolume;
    public UnityEngine.AudioClip[] FootstepAudioClips;
    public System.Single FootstepAudioVolume;
    public UnityEngine.Transform _camRootPlayer;
    public UnityEngine.Transform _camRootMount;
    public System.String _animation;
    public System.Boolean rootMotion;
    public UnityEngine.GameObject _mountUI;
    public System.Single walkSpeed;
    public System.Single runSpeed;
    public System.Boolean groundHugging;
    public UnityEngine.Transform _saddlePoint;
    public UnityEngine.Transform _dismountPoint;
    public UnityEngine.LayerMask groundLayer;
    public System.Int32 groupID;
    public System.Boolean autoSync;

    public CreatureMountZSerializer(string ZUID, string GOZUID) : base(ZUID, GOZUID)
    {       var instance = ZSerializer.ZSerialize.idMap[ZSerializer.ZSerialize.CurrentGroupID][ZUID];
         RotationSmoothTime = (System.Single)typeof(StupidHumanGames.CreatureMount).GetField("RotationSmoothTime").GetValue(instance);
         _mountTransform = (UnityEngine.Transform)typeof(StupidHumanGames.CreatureMount).GetField("_mountTransform").GetValue(instance);
         _randomAttackSound = (UnityEngine.AudioClip[])typeof(StupidHumanGames.CreatureMount).GetField("_randomAttackSound", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _randomAttackSoundVolume = (System.Single)typeof(StupidHumanGames.CreatureMount).GetField("_randomAttackSoundVolume", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _randomSound = (UnityEngine.AudioClip[])typeof(StupidHumanGames.CreatureMount).GetField("_randomSound", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _randomSoundVolume = (System.Single)typeof(StupidHumanGames.CreatureMount).GetField("_randomSoundVolume", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         FootstepAudioClips = (UnityEngine.AudioClip[])typeof(StupidHumanGames.CreatureMount).GetField("FootstepAudioClips", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         FootstepAudioVolume = (System.Single)typeof(StupidHumanGames.CreatureMount).GetField("FootstepAudioVolume", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _camRootPlayer = (UnityEngine.Transform)typeof(StupidHumanGames.CreatureMount).GetField("_camRootPlayer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _camRootMount = (UnityEngine.Transform)typeof(StupidHumanGames.CreatureMount).GetField("_camRootMount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _animation = (System.String)typeof(StupidHumanGames.CreatureMount).GetField("_animation", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         rootMotion = (System.Boolean)typeof(StupidHumanGames.CreatureMount).GetField("rootMotion", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _mountUI = (UnityEngine.GameObject)typeof(StupidHumanGames.CreatureMount).GetField("_mountUI", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         walkSpeed = (System.Single)typeof(StupidHumanGames.CreatureMount).GetField("walkSpeed", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         runSpeed = (System.Single)typeof(StupidHumanGames.CreatureMount).GetField("runSpeed", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         groundHugging = (System.Boolean)typeof(StupidHumanGames.CreatureMount).GetField("groundHugging", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _saddlePoint = (UnityEngine.Transform)typeof(StupidHumanGames.CreatureMount).GetField("_saddlePoint", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         _dismountPoint = (UnityEngine.Transform)typeof(StupidHumanGames.CreatureMount).GetField("_dismountPoint", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         groundLayer = (UnityEngine.LayerMask)typeof(StupidHumanGames.CreatureMount).GetField("groundLayer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         groupID = (System.Int32)typeof(ZSerializer.PersistentMonoBehaviour).GetField("groupID", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         autoSync = (System.Boolean)typeof(ZSerializer.PersistentMonoBehaviour).GetField("autoSync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
    }

    public override void RestoreValues(UnityEngine.Component component)
    {
         typeof(StupidHumanGames.CreatureMount).GetField("RotationSmoothTime").SetValue(component, RotationSmoothTime);
         typeof(StupidHumanGames.CreatureMount).GetField("_mountTransform").SetValue(component, _mountTransform);
         typeof(StupidHumanGames.CreatureMount).GetField("_randomAttackSound", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _randomAttackSound);
         typeof(StupidHumanGames.CreatureMount).GetField("_randomAttackSoundVolume", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _randomAttackSoundVolume);
         typeof(StupidHumanGames.CreatureMount).GetField("_randomSound", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _randomSound);
         typeof(StupidHumanGames.CreatureMount).GetField("_randomSoundVolume", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _randomSoundVolume);
         typeof(StupidHumanGames.CreatureMount).GetField("FootstepAudioClips", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, FootstepAudioClips);
         typeof(StupidHumanGames.CreatureMount).GetField("FootstepAudioVolume", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, FootstepAudioVolume);
         typeof(StupidHumanGames.CreatureMount).GetField("_camRootPlayer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _camRootPlayer);
         typeof(StupidHumanGames.CreatureMount).GetField("_camRootMount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _camRootMount);
         typeof(StupidHumanGames.CreatureMount).GetField("_animation", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _animation);
         typeof(StupidHumanGames.CreatureMount).GetField("rootMotion", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, rootMotion);
         typeof(StupidHumanGames.CreatureMount).GetField("_mountUI", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _mountUI);
         typeof(StupidHumanGames.CreatureMount).GetField("walkSpeed", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, walkSpeed);
         typeof(StupidHumanGames.CreatureMount).GetField("runSpeed", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, runSpeed);
         typeof(StupidHumanGames.CreatureMount).GetField("groundHugging", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, groundHugging);
         typeof(StupidHumanGames.CreatureMount).GetField("_saddlePoint", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _saddlePoint);
         typeof(StupidHumanGames.CreatureMount).GetField("_dismountPoint", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, _dismountPoint);
         typeof(StupidHumanGames.CreatureMount).GetField("groundLayer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, groundLayer);
         typeof(ZSerializer.PersistentMonoBehaviour).GetField("groupID", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, groupID);
         typeof(ZSerializer.PersistentMonoBehaviour).GetField("autoSync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, autoSync);
    }
}
}