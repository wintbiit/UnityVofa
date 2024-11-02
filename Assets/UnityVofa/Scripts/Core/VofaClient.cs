using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityVofa.Proto;

namespace UnityVofa
{
    public class VofaClient: TcpClient
    {
        private readonly BlockingCollection<ProtoChannel> _sendQueue;
        private readonly IVofaProto _proto;
        private readonly TimeSpan _sendInterval;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        public VofaClient(int bufferSize, IVofaProto proto, TimeSpan sendInterval)
        {
            _sendQueue = new BlockingCollection<ProtoChannel>(bufferSize);
            _proto = proto;
            _sendInterval = sendInterval;
        }
        
        public VofaClient(IVofaProto proto) : this(64, proto, TimeSpan.FromMilliseconds(100))
        {
        }

        public void Start()
        {
            Task.Run(() => SendLoop(_cts.Token), _cts.Token);
        }
        
        public void Write(ProtoChannel value)
        {
            _sendQueue.Add(value);
            _sendQueue.CompleteAdding();
        }

        public void Write(int channel, float value)
        { 
            Write(new ProtoChannel
            {
                Channel = channel,
                Value = value
            });
        }
        
        public void Write(IEnumerable<ProtoChannel> values)
        {
            foreach (var value in values)
            {
                _sendQueue.Add(value);
            }
            _sendQueue.CompleteAdding();
        }

        private async Task Flush(CancellationToken token)
        {
            var data = _sendQueue.GetConsumingEnumerable().ToArray();
            if (data.Length == 0)
            {
                return;
            }
            var buffer = _proto.Marshal(data);
            await GetStream().WriteAsync(buffer, 0, buffer.Length, token);
        }
        
        private async Task SendLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(_sendInterval, token);
                await Flush(token);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;
            
            _cts.Cancel();
            _cts.Dispose();
            _sendQueue.Dispose();
        }
    }
}