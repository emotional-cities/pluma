using Bonsai.Harp;
using System.Threading.Tasks;

namespace Harp.Pluma
{
    /// <inheritdoc/>
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the asynchronous API to configure and interface
        /// with Pluma devices on the specified serial port.
        /// </summary>
        /// <param name="portName">
        /// The name of the serial port used to communicate with the Harp device.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous initialization operation. The value of
        /// the <see cref="Task{TResult}.Result"/> parameter contains a new instance of
        /// the <see cref="AsyncDevice"/> class.
        /// </returns>
        public static async Task<AsyncDevice> CreateAsync(string portName)
        {
            var device = new AsyncDevice(portName);
            var whoAmI = await device.ReadWhoAmIAsync();
            if (whoAmI != Device.WhoAmI)
            {
                var errorMessage = string.Format(
                    "The device ID {1} on {0} was unexpected. Check whether a Pluma device is connected to the specified serial port.",
                    portName, whoAmI);
                throw new HarpException(errorMessage);
            }

            return device;
        }
    }

    /// <summary>
    /// Represents an asynchronous API to configure and interface with Pluma devices.
    /// </summary>
    public partial class AsyncDevice : Bonsai.Harp.AsyncDevice
    {
        internal AsyncDevice(string portName)
            : base(portName)
        {
        }

        /// <summary>
        /// Asynchronously reads the contents of the StreamsEnable register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Streams> ReadStreamsEnableAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StreamsEnable.Address));
            return StreamsEnable.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the StreamsEnable register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Streams>> ReadTimestampedStreamsEnableAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StreamsEnable.Address));
            return StreamsEnable.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the StreamsEnable register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteStreamsEnableAsync(Streams value)
        {
            var request = StreamsEnable.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the StreamsDisable register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Streams> ReadStreamsDisableAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StreamsDisable.Address));
            return StreamsDisable.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the StreamsDisable register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Streams>> ReadTimestampedStreamsDisableAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StreamsDisable.Address));
            return StreamsDisable.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the StreamsDisable register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteStreamsDisableAsync(Streams value)
        {
            var request = StreamsDisable.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the OximeterData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<OximeterDataPayload> ReadOximeterDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OximeterData.Address));
            return OximeterData.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the OximeterData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<OximeterDataPayload>> ReadTimestampedOximeterDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OximeterData.Address));
            return OximeterData.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EcgData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadEcgDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(EcgData.Address));
            return EcgData.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EcgData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedEcgDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(EcgData.Address));
            return EcgData.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the GsrData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadGsrDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(GsrData.Address));
            return GsrData.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the GsrData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedGsrDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(GsrData.Address));
            return GsrData.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AccelerometerData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadAccelerometerDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(AccelerometerData.Address));
            return AccelerometerData.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AccelerometerData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedAccelerometerDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(AccelerometerData.Address));
            return AccelerometerData.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PortDigitalInput register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalInputs> ReadPortDigitalInputAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PortDigitalInput.Address));
            return PortDigitalInput.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PortDigitalInput register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalInputs>> ReadTimestampedPortDigitalInputAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PortDigitalInput.Address));
            return PortDigitalInput.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the OutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputSet.Address));
            return OutputSet.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the OutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputSet.Address));
            return OutputSet.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the OutputSet register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteOutputSetAsync(DigitalOutputs value)
        {
            var request = OutputSet.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the OutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputClear.Address));
            return OutputClear.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the OutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputClear.Address));
            return OutputClear.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the OutputClear register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteOutputClearAsync(DigitalOutputs value)
        {
            var request = OutputClear.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }
    }
}
