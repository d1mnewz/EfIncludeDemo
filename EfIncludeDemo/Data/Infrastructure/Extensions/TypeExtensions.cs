using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EfIncludeDemo.Data.Infrastructure
{
public static class TypeExtensions
  {
    public static object GetDefaultValue(this Type type)
    {
      if (!type.IsValueType)
        return (object) null;
      return Activator.CreateInstance(type);
    }

    public static object GetNewInstance(this Type type)
    {
      return Activator.CreateInstance(type);
    }

    public static bool IsNullable(this Type type)
    {
      return type.IsGeneric(typeof (Nullable<>));
    }

    public static bool IsGeneric(this Type type)
    {
      return type.GetTypeInfo().IsGenericType;
    }

    public static Type BaseType(this Type type)
    {
      if ((object) type == null)
        return (Type) null;
      return type.GetTypeInfo().BaseType;
    }

    public static bool IsGeneric(this Type type, Type openType)
    {
      if (!type.IsGeneric())
        return false;
      if (type.GetGenericTypeDefinition() == openType)
        return true;
      if (type.BaseType() != (Type) null)
        return type.BaseType().IsGeneric(openType);
      return false;
    }

    public static bool Closes(this Type type, Type openType)
    {
      Type type1 = type.BaseType();
      if (type1 == (Type) null)
        return false;
      if (!type1.IsGeneric(openType))
        return type1.Closes(openType);
      return true;
    }

    public static bool IsOpenGeneric(this Type type)
    {
      TypeInfo typeInfo = type.GetTypeInfo();
      if (!typeInfo.IsGenericTypeDefinition)
        return typeInfo.ContainsGenericParameters;
      return true;
    }

    public static bool ImplementsInterfaceTemplate(this Type pluggedType, Type templateType)
    {
      if (pluggedType.IsConcrete())
        return ((IEnumerable<Type>) pluggedType.GetTypeInfo().GetInterfaces()).Any<Type>((Func<Type, bool>) (itfType =>
        {
          if (itfType.IsGeneric())
            return itfType.GetGenericTypeDefinition() == templateType;
          return false;
        }));
      return false;
    }

    public static bool ImplementsInterface<TInterface>(this Type type)
    {
      Type tInterface = typeof (TInterface);
      return ((IEnumerable<Type>) type.GetTypeInfo().GetInterfaces()).Any<Type>((Func<Type, bool>) (itfType => itfType == tInterface));
    }

    public static bool IsConcrete(this Type type)
    {
      TypeInfo typeInfo = type.GetTypeInfo();
      if (!typeInfo.IsAbstract)
        return !typeInfo.IsInterface;
      return false;
    }
  }
}