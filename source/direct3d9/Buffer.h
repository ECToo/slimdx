/*
* Copyright (c) 2007 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
#pragma once

#include "../DataStream.h"

namespace SlimDX
{
	namespace Direct3D9
	{
		public ref class BufferWrapper : public DirectXObject<ID3DXBuffer>
		{
		internal:
			static String^ MakeString( ID3DXBuffer* buffer )
			{
				if( buffer != NULL )
				{
					return gcnew String( (const char*) buffer->GetBufferPointer() );
				}
				else
				{
					return String::Empty;
				}
			}

		public:
			BufferWrapper( ID3DXBuffer* buffer ) : DirectXObject<ID3DXBuffer>( buffer )
			{
				if( buffer == NULL )
					throw gcnew ArgumentNullException( "buffer" );
			}

			property int Size
			{
				int get() { return m_Pointer->GetBufferSize(); }
			}

			property IntPtr DataPointer
			{
				IntPtr get() { return IntPtr( m_Pointer->GetBufferPointer() ); }
			}

			DataStream^ GetData()
			{
				return gcnew DataStream( m_Pointer->GetBufferPointer(), m_Pointer->GetBufferSize(), true, true );
			}

			virtual String^ ToString() override
			{
				return gcnew String( (const char*) m_Pointer->GetBufferPointer() );
			}
		};
	}
}