using StackExchange.Redis;
using System.Reflection;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Infrastructure.Common.Extensions
{
    public static class RedisExtention
    {
        public static HashEntry[] ToHashEntries(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            return properties
                .Where(x => x.GetValue(obj) != null) // <-- PREVENT NullReferenceException
                .Select
                (
                      property =>
                      {
                          object? propertyValue = property.GetValue(obj);
                          string? hashValue;

                          // This will detect if given property value is 
                          // enumerable, which is a good reason to serialize it
                          // as JSON!
                          if (propertyValue is IEnumerable<object>)
                          {
                              // So you use JSON.NET to serialize the property
                              // value as JSON
                              hashValue = JsonSerializer.Serialize(propertyValue);
                          }
                          else
                          {
                              hashValue = propertyValue?.ToString();
                          }

                          return new HashEntry(property.Name, hashValue);
                      }
                )
                .ToArray();
        }

        public static T? ConvertFromRedis<T>(this HashEntry[] hashEntries)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var obj = Activator.CreateInstance(typeof(T));
            foreach (var property in properties)
            {
                HashEntry entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name));
                if (entry.Equals(new HashEntry())) continue;

                if (property.PropertyType == typeof(Guid))
                {
                    if (Guid.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<bool>))
                {
                    if (bool.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<Int16>))
                {
                    if (Int16.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<UInt16>))
                {
                    if (UInt16.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<int>))
                {
                    if (int.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<uint>))
                {
                    if (uint.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<Int64>))
                {
                    if (Int64.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<UInt64>))
                {
                    if (UInt64.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<char>))
                {
                    if (char.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<sbyte>))
                {
                    if (sbyte.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<byte>))
                {
                    if (byte.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<Single>))
                {
                    if (Single.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<double>))
                {
                    if (double.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<Double>))
                {
                    if (Double.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<Decimal>))
                {
                    if (Decimal.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else if (property.PropertyType == typeof(Nullable<DateTime>))
                {
                    if (DateTime.TryParse(entry.Value.ToString(), out var res))
                    {
                        property.SetValue(obj, res);
                    }
                }
                else
                    property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), property.PropertyType));
            }
            return (T?)obj;
        }
    }
}
