using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities
{
    /// <summary>
    /// Lớp tiện ích để lấy danh sách các Type từ các Assembly được định nghĩa trước.
    /// </summary>
    public static class PredefineAssemblyUtil
    {
        /// <summary>
        /// Enum đại diện cho các loại Assembly.
        /// </summary>
        private enum AssemblyType
        {
            AssemblyCharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass
        }

        /// <summary>
        /// Lấy loại Assembly tương ứng với tên Assembly.
        /// </summary>
        /// <param name="assemblyName">Tên của Assembly.</param>
        /// <returns>Loại Assembly nếu tìm thấy, ngược lại trả về null.</returns>
        private static AssemblyType? GetAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                _ => null
            };
        }

        /// <summary>
        /// Lấy danh sách các Type kế thừa từ một interface cụ thể trong các Assembly được định nghĩa trước.
        /// </summary>
        /// <param name="interfaceType">Kiểu interface cần tìm kiếm.</param>
        /// <returns>Danh sách các Type thỏa mãn điều kiện.</returns>
        public static List<Type> GetTypes(Type interfaceType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            List<Type> types = new List<Type>();

            // Lấy danh sách các Type trong từng Assembly
            foreach (Assembly assembly in assemblies)
            {
                AssemblyType? assemblyType = GetAssemblyType(assembly.GetName().Name);
                if (assemblyType != null)
                {
                    assemblyTypes.Add((AssemblyType)assemblyType, assembly.GetTypes());
                }
            }

            // Thêm các Type từ Assembly-CSharp và Assembly-CSharp-firstpass vào danh sách kết quả
            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCharp], types, interfaceType);
            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharpFirstPass], types, interfaceType);

            return types;
        }

        /// <summary>
        /// Thêm các Type kế thừa từ interfaceType vào danh sách types.
        /// </summary>
        /// <param name="assembly">Mảng các Type trong Assembly.</param>
        /// <param name="types">Danh sách các Type kết quả.</param>
        /// <param name="interfaceType">Kiểu interface cần tìm kiếm.</param>
        private static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
        {
            if (assembly == null)
                return;

            foreach (Type type in assembly)
            {
                if (type != interfaceType && interfaceType.IsAssignableFrom(type))
                {
                    types.Add(type);
                }
            }
        }
    }
}