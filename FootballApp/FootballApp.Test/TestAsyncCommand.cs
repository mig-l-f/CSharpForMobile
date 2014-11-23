// <copyright file="TestAsyncCommand.cs" company="Miguel Fernandes">
//     Miguel Fernandes. All rights reserved.
// </copyright>
// <author>Miguel Fernandes</author>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using FootballApp.Core.ViewModel.Commands;

namespace FootballApp.Test
{
//#if DEBUG
//    [SetUpFixture]
//    public class SetupFixtureClass
//    {
//        [SetUp]
//        public void StartTesting()
//        {
//            System.Diagnostics.Debugger.Launch();
//        }
//    }
//#endif
    /// <summary>
    /// AsyncCommand unit tests
    /// </summary>
    [TestFixture, Category("AsyncCommandTests")]
    public class TestAsyncCommand
    {
        /// <summary>
        /// Tests the asynchronous command completes successfully.
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task TestAsyncCommandCompletesSuccessfully()
        {            
            AsyncCommand<string> target = new AsyncCommand<string>(() => Task.Factory.StartNew(() => 
            {
                return "Success";
            }));

            bool isCompletedEvent = false;
            target.Execution.PropertyChanged += (s, e) => 
            { 
                if (e.PropertyName.Equals("IsSuccessfullyCompleted")) 
                { 
                    isCompletedEvent = true; 
                } 
            }; 
            target.ExecuteAsync(null);
            while (target.Execution.IsNotCompleted) 
            {
            }

            Assert.AreEqual("Success", target.Execution.Result, "Result string is not the expected");
            Assert.IsTrue(isCompletedEvent, "Event successfully completed should have been raised");
        }
        
        /// <summary>
        /// Tests the asynchronous command catches exception.
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task TestAsyncCommandCatchesException()
        {
            AsyncCommand<string> target = new AsyncCommand<string>(() => Task.Factory.StartNew(() =>
            {
                throw new Exception("an exception occured");                
                return "Failed";
            }));
            bool isFaultedEvent = false;
            target.Execution.PropertyChanged += (s, e) => 
            { 
                if (e.PropertyName.Equals("IsFaulted")) 
                { 
                    isFaultedEvent = true; 
                } 
            };
            target.ExecuteAsync(null);
            while (target.Execution.IsNotCompleted) 
            {
            }

            Assert.IsTrue(target.Execution.IsFaulted, "task should be faulted");
            Assert.IsFalse(target.Execution.IsSuccessfullyCompleted, "task should have failed");
            Assert.AreEqual("an exception occured", target.Execution.ErrorMessage);
            Assert.IsTrue(isFaultedEvent, "Faulted event should have been raised");
        }

        /// <summary>
        /// Tests the asynchronous command in execution state is correct.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestAsyncCommandInExecutionStateIsCorrect()
        {
            AsyncCommand<string> target = new AsyncCommand<string>(() => Task.Factory.StartNew(() => 
            {
                return "Done";
            }));

            bool taskIsRunning = false;
            bool taskHasCompleted = false;
            int countNumberOfTimesIsRunningChanged = 0;

            target.Execution.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName.Equals("IsRunning"))
                {
                    if (!taskIsRunning)
                    {
                        taskIsRunning = true;
                    }
                    else 
                    {
                        taskIsRunning = false;
                    }
                    ++countNumberOfTimesIsRunningChanged;
                }
                if (e.PropertyName.Equals("IsCompleted"))
                {
                    taskHasCompleted = true;
                }
            };

            await target.ExecuteAsync(null);            
            while (target.Execution.IsNotCompleted)
            { }

            Assert.AreEqual(2, countNumberOfTimesIsRunningChanged, "IsRunning event should have been raised twice");
            Assert.AreEqual("Done", target.Execution.Result, "Task did not return the expected string");
            Assert.IsFalse(taskIsRunning, "Task should not be running");
            Assert.IsTrue(taskHasCompleted, "Task should have completed");
        }
    }
}
