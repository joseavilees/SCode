﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace SCode.WebApi.Application.Extensions
{
    public static class TaskExtension
    {
        /// <summary>
        /// TimeoutAfter
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns></returns>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();

            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return await task;  // Very important in order to propagate exceptions
            }
            else
            {
                throw new TimeoutException($"{nameof(TimeoutAfter)}: " +
                    $"The operation has timed out after {timeout:mm\\:ss}");
            }
        }
    }
}
