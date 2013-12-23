﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using DequeNet.Test.Common;

namespace DequeNet.Unit
{
    public partial class ConcurrentDequeFixture
    {
        [Fact]
        public void DequeHasNoItemsByDefault()
        {
            //Act
            var deque = new ConcurrentDeque<int>();

            //Assert
            var anchor = deque._anchor;
            Assert.Null(anchor._left);
            Assert.Null(anchor._right);
        }

        [Fact]
        public void DequeIsStableByDefault()
        {
            //Act
            var deque = new ConcurrentDeque<int>();

            //Assert
            var anchor = deque._anchor;
            Assert.Equal(ConcurrentDeque<int>.DequeStatus.Stable, anchor._status);
        }

        [Fact]
        public void PushRightAppendsNodeToEmptyList()
        {
            //Arrange
            const int value = 5;
            var deque = new ConcurrentDeque<int>();

            //Act
            deque.PushRight(value);

            //Assert
            var anchor = deque._anchor;
            Assert.NotNull(anchor._right);
            Assert.NotNull(anchor._left);
            Assert.Same(anchor._left, anchor._right);
            Assert.Equal(value, anchor._right._value);
        }

        [Fact]
        public void PushRightAppendsNodeToNonEmptyList()
        {
            //Arrange
            const int value = 5;
            var deque = new ConcurrentDeque<int>();
            deque.PushRight(1);

            //Act
            deque.PushRight(value);

            //Assert
            var anchor = deque._anchor;
            var newNode = anchor._right;
            Assert.NotNull(newNode);
            Assert.Equal(value, newNode._value);
        }

        [Fact]
        public void PushRightKeepsReferenceToPreviousRightNode()
        {
            //Arrange
            const int prevValue = 1;
            const int value = 5;
            var deque = new ConcurrentDeque<int>();
            deque.PushRight(prevValue);

            //Act
            deque.PushRight(value);

            //Assert
            var anchor = deque._anchor;
            var newNode = anchor._right;
            Assert.Equal(prevValue, newNode._left._value);
        }

        [Fact]
        public void PushRightStabilizesDeque()
        {
            //Arrange
            const int prevValue = 1;
            const int value = 5;
            var deque = new ConcurrentDeque<int>();
            deque.PushRight(prevValue);

            //Act
            deque.PushRight(value);

            //Assert
            var anchor = deque._anchor;
            var newNode = anchor._right;
            Assert.Same(newNode, newNode._left._right);
            Assert.Equal(ConcurrentDeque<int>.DequeStatus.Stable, anchor._status);
        }

        [Fact]
        public void TryPopRightFailsOnEmptyDeque()
        {
            //Arrange
            var deque = new ConcurrentDeque<int>();
            
            //Act & Assert
            int item;
            Assert.False(deque.TryPopRight(out item));
            Assert.Equal(item, default(int));
        }

        [Fact]
        public void TryPopReturnsTheLastRemainingItem()
        {
            //Arrange
            var deque = new ConcurrentDeque<int>();
            deque.PushRight(1);

            //Act & Assert
            int item;
            Assert.True(deque.TryPopRight(out item));
            Assert.Equal(item, 1);

            long nodesCount = deque.GetNodes().LongCount();
            Assert.Equal(0, nodesCount);
        }

        [Fact]
        public void TryPopReturnsTheRightmostItem()
        {
            //Arrange
            var deque = new ConcurrentDeque<int>();
            deque.PushRight(1);
            deque.PushRight(3);
            deque.PushRight(5);

            //Act & Assert
            int item;
            Assert.True(deque.TryPopRight(out item));
            Assert.Equal(item, 5);

            long nodesCount = deque.GetNodes().LongCount();
            Assert.Equal(2, nodesCount);
        }
    }
}