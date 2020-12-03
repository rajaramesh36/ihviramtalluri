namespace InstaHangouts.Interfaces
{
    /// <summary>
    /// Service Wrapper
    /// </summary>
    /// <typeparam name="H">The Header</typeparam>
    /// <typeparam name="B">The Body</typeparam>
    /// 
    public class ServiceWrapper<H, B>
    {
        /// <summary>
        /// The packet
        /// </summary>
        public Packet<H, B> packet = new Packet<H, B>();
    }

    /// <summary>
    /// Class Packet.
    /// </summary>
    /// <typeparam name="H">The Header</typeparam>
    /// <typeparam name="B">The Body</typeparam>
    /// 
    public class Packet<H, B>
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public H Header { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public B Body { get; set; }
    }

    /// <summary>
    /// Class Header.
    /// </summary>
    /// 
    public class Header
    {
        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>The response code.</value>
        public string ResponseCode { get; set; }

        /// <summary>
        /// Success or Failed Message showed on UI
        /// </summary>
        /// <value>The response message.</value>
        public string ResponseMessage { get; set; }
        /// <summary>
        /// More info on Error, mostly required to developer
        /// </summary>
        public string Details { get; set; } = string.Empty;
    }
}
