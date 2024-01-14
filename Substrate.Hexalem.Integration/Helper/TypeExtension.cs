using Substrate.NetApi.Model.Types;
using System;

namespace Substrate.Hexalem.Integration.Helper
{
    /// <summary>
    /// Type extension
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Cast a type to another type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static T As<T>(this IType sender)
        {
            if (sender is T typed)
            {
                return typed;
            }

            throw new InvalidCastException();
        }
    }
}