#pragma once

#include "Resource.h"
#include "GraphicsStream.h"

using namespace System;

namespace SlimDX
{
	namespace Direct3D
	{
		public ref class VertexBuffer sealed : public Resource
		{
		private:
			IDirect3DVertexBuffer9* m_Buffer;

		internal:
			property IDirect3DVertexBuffer9* InternalPointer
			{
				IDirect3DVertexBuffer9* get() { return m_Buffer; }
			}

			property IDirect3DResource9* ResourcePointer
			{
				virtual IDirect3DResource9* get() override { return m_Buffer; }
			}

			property IUnknown* ComPointer
			{
				virtual IUnknown* get() override { return m_Buffer; }
				virtual void set( IUnknown* value ) override { m_Buffer = (IDirect3DVertexBuffer9*) value; }
			}

		public:
			VertexBuffer( IDirect3DVertexBuffer9* buffer );
			VertexBuffer( Device^ device, int sizeBytes, Usage usage, VertexFormats format, Pool pool );

			GraphicsStream^ Lock( int offset, int size, LockFlags flags );
			GraphicsStream^ Lock( int offset, LockFlags flags ) { return Lock( offset, 0, flags ); }
			void Unlock();
		};
	}
}
