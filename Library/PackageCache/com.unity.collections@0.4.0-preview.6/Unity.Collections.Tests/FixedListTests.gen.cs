

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     TextTransform Samples/Packages/com.unity.entities/Unity.Entities/FixedListTests.tt
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

public class FixedListTests
{
    

    [Test]
    public void FixedListByte32HasExpectedLayout()
    {
        var actual = new FixedListByte32();
        for(var i = 0; i < 30; ++i)
          actual.Add((byte)i);
        unsafe
        {        
            var e = stackalloc byte[32];
            e[0] = (byte)((30 >> 0) & 0xFF);
            e[1] = (byte)((30 >> 8) & 0xFF);
            for(var i = 0; i < 30; ++i)
            {
              var s = (byte)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(byte) * i, &s, sizeof(byte));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 32));
        }
    }
    
    [Test]
    public void FixedListByte32HasExpectedCapacity()
    {
        var list = new FixedListByte32();
        var expectedCapacity = 30;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((byte)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((byte)expectedCapacity); });
    }
                
    [Test]
    public void FixedListByte32InsertRange()
    {
        var list = new FixedListByte32();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListByte32RemoveRange()
    {
        var list = new FixedListByte32();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListByte32Sort()
    {
        var list = new FixedListByte32();
        for(var i = 0; i < 5; ++i)
          list.Add((byte)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListByte32ToFixedListByte64()
    {
        var a = new FixedListByte32();
        for(var i = 0; i < 30; ++i)
            a.Add((byte)i);
        var b = new FixedListByte64(a);
        for(var i = 0; i < 30; ++i)
            Assert.AreEqual((byte)i, b[i]);
    }
    [Test]
    public void FixedListByte32ToFixedListByte128()
    {
        var a = new FixedListByte32();
        for(var i = 0; i < 30; ++i)
            a.Add((byte)i);
        var b = new FixedListByte128(a);
        for(var i = 0; i < 30; ++i)
            Assert.AreEqual((byte)i, b[i]);
    }
    

    [Test]
    public void FixedListByte64HasExpectedLayout()
    {
        var actual = new FixedListByte64();
        for(var i = 0; i < 62; ++i)
          actual.Add((byte)i);
        unsafe
        {        
            var e = stackalloc byte[64];
            e[0] = (byte)((62 >> 0) & 0xFF);
            e[1] = (byte)((62 >> 8) & 0xFF);
            for(var i = 0; i < 62; ++i)
            {
              var s = (byte)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(byte) * i, &s, sizeof(byte));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 64));
        }
    }
    
    [Test]
    public void FixedListByte64HasExpectedCapacity()
    {
        var list = new FixedListByte64();
        var expectedCapacity = 62;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((byte)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((byte)expectedCapacity); });
    }
                
    [Test]
    public void FixedListByte64InsertRange()
    {
        var list = new FixedListByte64();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListByte64RemoveRange()
    {
        var list = new FixedListByte64();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListByte64Sort()
    {
        var list = new FixedListByte64();
        for(var i = 0; i < 5; ++i)
          list.Add((byte)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListByte64ToFixedListByte32()
    {
        var a = new FixedListByte64();
        for(var i = 0; i < 62; ++i)
            a.Add((byte)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListByte32(a); } );
    }
    [Test]
    public void FixedListByte64ToFixedListByte128()
    {
        var a = new FixedListByte64();
        for(var i = 0; i < 62; ++i)
            a.Add((byte)i);
        var b = new FixedListByte128(a);
        for(var i = 0; i < 62; ++i)
            Assert.AreEqual((byte)i, b[i]);
    }
    

    [Test]
    public void FixedListByte128HasExpectedLayout()
    {
        var actual = new FixedListByte128();
        for(var i = 0; i < 126; ++i)
          actual.Add((byte)i);
        unsafe
        {        
            var e = stackalloc byte[128];
            e[0] = (byte)((126 >> 0) & 0xFF);
            e[1] = (byte)((126 >> 8) & 0xFF);
            for(var i = 0; i < 126; ++i)
            {
              var s = (byte)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(byte) * i, &s, sizeof(byte));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 128));
        }
    }
    
    [Test]
    public void FixedListByte128HasExpectedCapacity()
    {
        var list = new FixedListByte128();
        var expectedCapacity = 126;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((byte)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((byte)expectedCapacity); });
    }
                
    [Test]
    public void FixedListByte128InsertRange()
    {
        var list = new FixedListByte128();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListByte128RemoveRange()
    {
        var list = new FixedListByte128();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListByte128Sort()
    {
        var list = new FixedListByte128();
        for(var i = 0; i < 5; ++i)
          list.Add((byte)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListByte128ToFixedListByte32()
    {
        var a = new FixedListByte128();
        for(var i = 0; i < 126; ++i)
            a.Add((byte)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListByte32(a); } );
    }
    [Test]
    public void FixedListByte128ToFixedListByte64()
    {
        var a = new FixedListByte128();
        for(var i = 0; i < 126; ++i)
            a.Add((byte)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListByte64(a); } );
    }
    

    [Test]
    public void FixedListInt32HasExpectedLayout()
    {
        var actual = new FixedListInt32();
        for(var i = 0; i < 7; ++i)
          actual.Add((int)i);
        unsafe
        {        
            var e = stackalloc byte[32];
            e[0] = (byte)((7 >> 0) & 0xFF);
            e[1] = (byte)((7 >> 8) & 0xFF);
            for(var i = 0; i < 7; ++i)
            {
              var s = (int)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(int) * i, &s, sizeof(int));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 32));
        }
    }
    
    [Test]
    public void FixedListInt32HasExpectedCapacity()
    {
        var list = new FixedListInt32();
        var expectedCapacity = 7;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((int)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((int)expectedCapacity); });
    }
                
    [Test]
    public void FixedListInt32InsertRange()
    {
        var list = new FixedListInt32();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListInt32RemoveRange()
    {
        var list = new FixedListInt32();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListInt32Sort()
    {
        var list = new FixedListInt32();
        for(var i = 0; i < 5; ++i)
          list.Add((int)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListInt32ToFixedListInt64()
    {
        var a = new FixedListInt32();
        for(var i = 0; i < 7; ++i)
            a.Add((int)i);
        var b = new FixedListInt64(a);
        for(var i = 0; i < 7; ++i)
            Assert.AreEqual((int)i, b[i]);
    }
    [Test]
    public void FixedListInt32ToFixedListInt128()
    {
        var a = new FixedListInt32();
        for(var i = 0; i < 7; ++i)
            a.Add((int)i);
        var b = new FixedListInt128(a);
        for(var i = 0; i < 7; ++i)
            Assert.AreEqual((int)i, b[i]);
    }
    

    [Test]
    public void FixedListInt64HasExpectedLayout()
    {
        var actual = new FixedListInt64();
        for(var i = 0; i < 15; ++i)
          actual.Add((int)i);
        unsafe
        {        
            var e = stackalloc byte[64];
            e[0] = (byte)((15 >> 0) & 0xFF);
            e[1] = (byte)((15 >> 8) & 0xFF);
            for(var i = 0; i < 15; ++i)
            {
              var s = (int)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(int) * i, &s, sizeof(int));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 64));
        }
    }
    
    [Test]
    public void FixedListInt64HasExpectedCapacity()
    {
        var list = new FixedListInt64();
        var expectedCapacity = 15;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((int)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((int)expectedCapacity); });
    }
                
    [Test]
    public void FixedListInt64InsertRange()
    {
        var list = new FixedListInt64();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListInt64RemoveRange()
    {
        var list = new FixedListInt64();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListInt64Sort()
    {
        var list = new FixedListInt64();
        for(var i = 0; i < 5; ++i)
          list.Add((int)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListInt64ToFixedListInt32()
    {
        var a = new FixedListInt64();
        for(var i = 0; i < 15; ++i)
            a.Add((int)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListInt32(a); } );
    }
    [Test]
    public void FixedListInt64ToFixedListInt128()
    {
        var a = new FixedListInt64();
        for(var i = 0; i < 15; ++i)
            a.Add((int)i);
        var b = new FixedListInt128(a);
        for(var i = 0; i < 15; ++i)
            Assert.AreEqual((int)i, b[i]);
    }
    

    [Test]
    public void FixedListInt128HasExpectedLayout()
    {
        var actual = new FixedListInt128();
        for(var i = 0; i < 31; ++i)
          actual.Add((int)i);
        unsafe
        {        
            var e = stackalloc byte[128];
            e[0] = (byte)((31 >> 0) & 0xFF);
            e[1] = (byte)((31 >> 8) & 0xFF);
            for(var i = 0; i < 31; ++i)
            {
              var s = (int)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(int) * i, &s, sizeof(int));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 128));
        }
    }
    
    [Test]
    public void FixedListInt128HasExpectedCapacity()
    {
        var list = new FixedListInt128();
        var expectedCapacity = 31;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((int)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((int)expectedCapacity); });
    }
                
    [Test]
    public void FixedListInt128InsertRange()
    {
        var list = new FixedListInt128();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListInt128RemoveRange()
    {
        var list = new FixedListInt128();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListInt128Sort()
    {
        var list = new FixedListInt128();
        for(var i = 0; i < 5; ++i)
          list.Add((int)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListInt128ToFixedListInt32()
    {
        var a = new FixedListInt128();
        for(var i = 0; i < 31; ++i)
            a.Add((int)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListInt32(a); } );
    }
    [Test]
    public void FixedListInt128ToFixedListInt64()
    {
        var a = new FixedListInt128();
        for(var i = 0; i < 31; ++i)
            a.Add((int)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListInt64(a); } );
    }
    

    [Test]
    public void FixedListFloat32HasExpectedLayout()
    {
        var actual = new FixedListFloat32();
        for(var i = 0; i < 7; ++i)
          actual.Add((float)i);
        unsafe
        {        
            var e = stackalloc byte[32];
            e[0] = (byte)((7 >> 0) & 0xFF);
            e[1] = (byte)((7 >> 8) & 0xFF);
            for(var i = 0; i < 7; ++i)
            {
              var s = (float)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(float) * i, &s, sizeof(float));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 32));
        }
    }
    
    [Test]
    public void FixedListFloat32HasExpectedCapacity()
    {
        var list = new FixedListFloat32();
        var expectedCapacity = 7;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((float)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((float)expectedCapacity); });
    }
                
    [Test]
    public void FixedListFloat32InsertRange()
    {
        var list = new FixedListFloat32();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListFloat32RemoveRange()
    {
        var list = new FixedListFloat32();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListFloat32Sort()
    {
        var list = new FixedListFloat32();
        for(var i = 0; i < 5; ++i)
          list.Add((float)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListFloat32ToFixedListFloat64()
    {
        var a = new FixedListFloat32();
        for(var i = 0; i < 7; ++i)
            a.Add((float)i);
        var b = new FixedListFloat64(a);
        for(var i = 0; i < 7; ++i)
            Assert.AreEqual((float)i, b[i]);
    }
    [Test]
    public void FixedListFloat32ToFixedListFloat128()
    {
        var a = new FixedListFloat32();
        for(var i = 0; i < 7; ++i)
            a.Add((float)i);
        var b = new FixedListFloat128(a);
        for(var i = 0; i < 7; ++i)
            Assert.AreEqual((float)i, b[i]);
    }
    

    [Test]
    public void FixedListFloat64HasExpectedLayout()
    {
        var actual = new FixedListFloat64();
        for(var i = 0; i < 15; ++i)
          actual.Add((float)i);
        unsafe
        {        
            var e = stackalloc byte[64];
            e[0] = (byte)((15 >> 0) & 0xFF);
            e[1] = (byte)((15 >> 8) & 0xFF);
            for(var i = 0; i < 15; ++i)
            {
              var s = (float)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(float) * i, &s, sizeof(float));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 64));
        }
    }
    
    [Test]
    public void FixedListFloat64HasExpectedCapacity()
    {
        var list = new FixedListFloat64();
        var expectedCapacity = 15;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((float)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((float)expectedCapacity); });
    }
                
    [Test]
    public void FixedListFloat64InsertRange()
    {
        var list = new FixedListFloat64();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListFloat64RemoveRange()
    {
        var list = new FixedListFloat64();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListFloat64Sort()
    {
        var list = new FixedListFloat64();
        for(var i = 0; i < 5; ++i)
          list.Add((float)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListFloat64ToFixedListFloat32()
    {
        var a = new FixedListFloat64();
        for(var i = 0; i < 15; ++i)
            a.Add((float)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListFloat32(a); } );
    }
    [Test]
    public void FixedListFloat64ToFixedListFloat128()
    {
        var a = new FixedListFloat64();
        for(var i = 0; i < 15; ++i)
            a.Add((float)i);
        var b = new FixedListFloat128(a);
        for(var i = 0; i < 15; ++i)
            Assert.AreEqual((float)i, b[i]);
    }
    

    [Test]
    public void FixedListFloat128HasExpectedLayout()
    {
        var actual = new FixedListFloat128();
        for(var i = 0; i < 31; ++i)
          actual.Add((float)i);
        unsafe
        {        
            var e = stackalloc byte[128];
            e[0] = (byte)((31 >> 0) & 0xFF);
            e[1] = (byte)((31 >> 8) & 0xFF);
            for(var i = 0; i < 31; ++i)
            {
              var s = (float)i;
              UnsafeUtility.MemCpy(e + 2 + sizeof(float) * i, &s, sizeof(float));
            }
            Assert.AreEqual(0, UnsafeUtility.MemCmp(e, &actual.length, 128));
        }
    }
    
    [Test]
    public void FixedListFloat128HasExpectedCapacity()
    {
        var list = new FixedListFloat128();
        var expectedCapacity = 31;
        for(int i = 0; i < expectedCapacity; ++i)
            list.Add((float)i);
		Assert.Throws<IndexOutOfRangeException> (() => { list.Add((float)expectedCapacity); });
    }
                
    [Test]
    public void FixedListFloat128InsertRange()
    {
        var list = new FixedListFloat128();
        list.Add(0);        
        list.Add(3);        
        list.Add(4);
        list.InsertRange(1,2);
        list[1] = 1;
        list[2] = 2;
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListFloat128RemoveRange()
    {
        var list = new FixedListFloat128();
        list.Add(0);        
        list.Add(3);        
        list.Add(3);        
        list.Add(1);        
        list.Add(2);
        list.RemoveRange(1,2);
        for(var i = 0; i < 3; ++i)
            Assert.AreEqual(i, list[i]);        
    }

    [Test]
    public void FixedListFloat128Sort()
    {
        var list = new FixedListFloat128();
        for(var i = 0; i < 5; ++i)
          list.Add((float)(4-i));
        list.Sort();
        for(var i = 0; i < 5; ++i)
            Assert.AreEqual(i, list[i]);                
    }
        
    [Test]
    public void FixedListFloat128ToFixedListFloat32()
    {
        var a = new FixedListFloat128();
        for(var i = 0; i < 31; ++i)
            a.Add((float)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListFloat32(a); } );
    }
    [Test]
    public void FixedListFloat128ToFixedListFloat64()
    {
        var a = new FixedListFloat128();
        for(var i = 0; i < 31; ++i)
            a.Add((float)i);
        Assert.Throws<IndexOutOfRangeException> (() => { var b = new FixedListFloat64(a); } );
    }
       

}
