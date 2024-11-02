using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityVofa.Proto
{
    public class JustFloat: IVofaProto
    {
        public static readonly byte[] FrameTail = {0x00, 0x00, 0x80, 0x7f};

        public byte[] Marshal(ProtoChannel[] data)
        {
            var maxChannel = data.Max(x => x.Channel);
            var buffer = new byte[4 * maxChannel + 4];
            foreach (var channel in data)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(channel.Value), 0, buffer, 4 * channel.Channel, 4);
            }
            
            Buffer.BlockCopy(FrameTail, 0, buffer, 4 * maxChannel, 4);
            
            return buffer;
        }
    }
}