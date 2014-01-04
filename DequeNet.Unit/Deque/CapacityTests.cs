﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DequeNet.Unit.Deque
{
    public class CapacityTests
    {
        [Fact]
        public void SettingTo_LessThanCount_ThrowsException()
        {
            var deque = new Deque<int>();
            deque.PushRight(5);

            Assert.Throws<ArgumentOutOfRangeException>(() => deque.Capacity = 0);
        }

        [Fact]
        public void SettingTo_HigherThanCount_IncreasesCapacity()
        {
            var deque = new Deque<int>();
            deque.Capacity = 6;

            Assert.Equal(6, deque.Capacity);
        }

        [Fact]
        public void SettingTo_CurrentCapacity_DoesNothing()
        {
            var deque = new Deque<int>(5);
            deque.Capacity = 5;
            Assert.Equal(5, deque.Capacity);
        }

        [Fact]
        public void Setting_CopiesItems()
        {
            var deque = new Deque<int>(new[] {1, 2, 3, 4});
            deque.PopLeft();

            deque.Capacity = 7;

            Assert.Equal(new[] {2, 3, 4}, deque);
        }

        [Fact]
        public void Setting_WhenDequeLoopsAroundArray_CopiesItems()
        {
            var deque = new Deque<int>(new[] {1, 2, 3, 4});
            deque.PopLeft();
            deque.PopLeft();
            deque.PushRight(5);

            deque.Capacity = 3;

            Assert.Equal(new[]{3,4,5}, deque);
        } 
    }
}