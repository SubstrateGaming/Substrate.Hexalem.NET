using Substrate.Hexalem.NET.NetApiExt.Generated.Model.hexalem_runtime;

namespace Substrate.Integration.Call
{
    /// <summary>
    /// Call interface to be implemented by all calls
    /// </summary>
    public interface ICall
    {
        /// <summary>
        /// Convert the call to a runtime call
        /// </summary>
        /// <returns></returns>
        EnumRuntimeCall ToCall();
    }
}