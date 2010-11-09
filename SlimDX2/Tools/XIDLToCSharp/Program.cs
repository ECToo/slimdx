﻿// Copyright (c) 2007-2010 SlimDX Group
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Globalization;
using System.IO;
using System.Text;
using SlimDX2.Tools.XIDL;

namespace SlimDX2.Tools.XIDLToCSharp
{
    internal partial class Program
    {
        public CppIncludeGroup group;
        public CSharpGenerator gen;

    
        /// <summary>
        /// Run the XIDL To CSharp generator
        /// </summary>
        public void Run()
        {
            string fileNameXIDL = "directx.xidl";

            if (!File.Exists(fileNameXIDL))
            {
                Console.WriteLine("File {0} not found. You must run HeaderToXIDL before running XIDLToCSharp");
                Environment.Exit(1);
            }

            // Instantiate main objects
            group = CppIncludeGroup.Read(fileNameXIDL);
            gen = new CSharpGenerator(group);

            // For all methods "GetXXX", convert parameters with [None] attribute and pointer to [Out] attribute
            // TODO remove this and apply one-to-one mapping
            group.ModifyToGetMethods(".*");

            //  Global Rename
            group.TagName<CppEnum>(@"^D3D\d?\d?(.+)", "$1", false);
            group.TagName<CppEnum>(@"^D3DX\d?\d?(.+)", "$1", false);
            group.TagName<CppStruct>(@"^D3D\d?\d?(.+)", "$1", false);
            group.TagName<CppStruct>(@"^D3DX\d?\d?(.+)", "$1", false);
            group.TagName<CppFunction>(@"^D3D\d?\d?(.+)", "$1", false);
            group.TagName<CppFunction>(@"^D3DX\d?\d?(.+)", "$1", false);
            group.TagName<CppInterface>(@"^ID3D\d?\d?(.+)", "$1", false);
            group.TagName<CppInterface>(@"^ID3DX\d?\d?(.+)", "$1", false);

            // -----------------------------------------------------------------------
            // Mapping order is important!!!
            // -----------------------------------------------------------------------
            MapWin32();

            MapDXGI();

            MapDirect3DCommon();
            MapD3DCompiler();

            MapDirect3D9();
            MapDirect3D10AndDirect3D11();
            
            MapDirect2D1();
            MapDirectWrite();

            gen.GeneratedPath = @"..\..\..\Sources\";

            gen.Generate();

            gen.Dump("slimdx2.csv");

            //DumpEnumItems("direct3D9_enums.txt");
        }

        //public void DumpEnumItems(string fileName)
        //{
        //    StreamWriter log = new StreamWriter(fileName, false, Encoding.ASCII);

        //    var assemblySlimDX = typeof(SlimDX.Direct3D11.Device).Assembly;

        //    foreach (var assembly in gen.Assemblies)
        //    {
        //        foreach (var ns in assembly.Namespaces)
        //        {
        //            if (ns.Name == "SlimDX2.Direct3D9")
        //            {
        //                foreach (var cSharpEnum in ns.Enums)
        //                {
        //                    Type slimdxType = assemblySlimDX.GetType("SlimDX.Direct3D9." + cSharpEnum.Name);

        //                    if (slimdxType != null)
        //                    {
        //                        int i = 0;
        //                        foreach (var enumItem in cSharpEnum.EnumItems)
        //                        {
        //                            try
        //                            {
        //                                int value = Evaluator.EvalToInteger(string.IsNullOrEmpty(enumItem.Value) ? "" + i : enumItem.Value);

        //                                string name = slimdxType.GetEnumName(value);
        //                                log.WriteLine("group.Tag<CppEnumItem>(@\"^{0}$\",\"{1}\");", enumItem.CppElementName, name);
        //                            } catch (Exception ex)
        //                            {
        //                                Console.WriteLine(ex);
        //                            }
        //                            i++;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("Enum not found {0}", "SlimDX.Direct3D9." + cSharpEnum.Name);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    log.Close();
        //}



        /// <summary>
        /// Main XIDLToCSharp
        /// </summary>
        /// <param name="args">Command line args.</param>
        private unsafe static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }
    }
}