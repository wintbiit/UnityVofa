using System.Globalization;
using System.Linq;
using System.Text;

namespace UnityVofa.Proto
{
    public class FireWater: IVofaProto
    {
        public byte[] Marshal(ProtoChannel[] data)
        {
            var builder = new StringBuilder();
            var largestChannel = data.Max(x => x.Channel);
            for (var i = 0; i <= largestChannel; i++)
            {
                var channel = data.FirstOrDefault(x => x.Channel == i);
                builder.Append(channel.Value.ToString(CultureInfo.InvariantCulture));
                if (i != largestChannel)
                {
                    builder.Append(",");
                }
            }
            
            builder.Append("\n");
            
            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}