namespace UnityVofa.Proto
{
    public interface IVofaProto
    {
        public byte[] Marshal(ProtoChannel[] data);
    }

    public struct ProtoChannel
    {
        public float Value;
        public int Channel;
    }
    
    public enum VofaProtoType
    {
        JustFloat,
        RawData,
        FireWater
    }

    public static class VofaProto
    {
        public static IVofaProto Create(this VofaProtoType type)
        {
            return type switch
            {
                VofaProtoType.JustFloat => new JustFloat(),
                VofaProtoType.FireWater => new FireWater(),
                _ => null
            };
        }
    }
}