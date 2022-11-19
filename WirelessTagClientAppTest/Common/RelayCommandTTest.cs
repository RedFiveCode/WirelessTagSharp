using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;
using WirelessTagClientApp.Common;

namespace WirelessTagClientApp.Test.Common
{
    /// <summary>
    /// Unit tests for the <see cref="RelayCommandT"/> class
    /// </summary>
    [TestClass]
    public class RelayCommandTTest
    {
        [TestMethod]
        public void RelayCommandT_Implements_ICommand()
        {
            var target = new RelayCommandT<ExampleViewModel>(p => Execute(p));

            Assert.IsInstanceOfType(target, typeof(ICommand));
        }

        [TestMethod]
        public void RelayCommandT_Execute_CallsExecuteDelegate()
        {
            // arrange
            bool executeCalled = true;

            var target = new RelayCommandT<ExampleViewModel>(p =>
            {
                executeCalled = true;
            });
            var vm = new ExampleViewModel();

            // act
            target.Execute(vm);

            // assert
            Assert.IsTrue(executeCalled);
        }

        [TestMethod]
        public void RelayCommandT_Execute_ParameterIsIncorrectType_ReturnsExpectedValue()
        {
            // arrange
            bool executeCalled = false;
            bool executeParameterIsNull = false;

            var target = new RelayCommandT<ExampleViewModel>(p =>
            {
                executeCalled = true;
                executeParameterIsNull = (p == null);
            });

            var vm = new ExampleViewModel();

            // act
            target.Execute(this);

            // assert
            Assert.IsTrue(executeCalled);
            Assert.IsTrue(executeParameterIsNull);
        }

        [TestMethod]
        public void RelayCommandT_Execute_ParameterIsCorrectType_ReturnsExpectedValue()
        {
            // arrange
            bool executeCalled = false;
            bool executeParameterIsNull = false;

            var target = new RelayCommandT<ExampleViewModel>(p =>
            {
                executeCalled = true;
                executeParameterIsNull = (p == null);
            });

            var vm = new ExampleViewModel();

            // act
            target.Execute(vm);

            // assert
            Assert.IsTrue(executeCalled);
            Assert.IsFalse(executeParameterIsNull);
        }

        [TestMethod]
        public void RelayCommandT_CanExecute_CallsCanExecuteDelegate()
        {
            // arrange
            bool _canExecuteCalled = false;

            var target = new RelayCommandT<ExampleViewModel>(p => Execute(p), p =>
            {
                _canExecuteCalled = true;

                return true;
            });

            var vm = new ExampleViewModel();

            // act
            var result = target.CanExecute(vm);

            // assert
            Assert.IsTrue(_canExecuteCalled);
        }

        [TestMethod]
        public void RelayCommandT_CanExecute_ReturnsExpectedValue()
        {
            // arrange
            var target = new RelayCommandT<ExampleViewModel>(p => Execute(p), p => CanExecute(p));
            var vm = new ExampleViewModel();

            // act
            var result = target.CanExecute(vm);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RelayCommandT_CanExecute_ParameterIsIncorrectType_ReturnsExpectedValue()
        {
            // arrange
            bool _canExecuteCalled = false;
            bool _canExecuteParameterIsNull = false;

            var target = new RelayCommandT<ExampleViewModel>(p => Execute(p), p =>
            {
                _canExecuteCalled = true;
                _canExecuteParameterIsNull = (p == null);

                return true;
            });

            var vm = new ExampleViewModel();

            // act
            var result = target.CanExecute(this);

            // assert
            Assert.IsTrue(result);
            Assert.IsTrue(_canExecuteCalled);
            Assert.IsTrue(_canExecuteParameterIsNull);
        }

        [TestMethod]
        public void RelayCommandT_CanExecute_ParameterIsCorrectType_ReturnsExpectedValue()
        {
            // arrange
            bool _canExecuteCalled = false;
            bool _canExecuteParameterIsNull = false;

            var target = new RelayCommandT<ExampleViewModel>(p => Execute(p), p =>
            {
                _canExecuteCalled = true;
                _canExecuteParameterIsNull = (p == null);

                return true;
            });

            var vm = new ExampleViewModel();

            // act
            var result = target.CanExecute(vm);

            // assert
            Assert.IsTrue(result);
            Assert.IsTrue(_canExecuteCalled);
            Assert.IsFalse(_canExecuteParameterIsNull);
        }

        private bool CanExecute(ExampleViewModel p)
        {
            return true;
        }

        private void Execute(ExampleViewModel p)
        {
            // do something
        }
    }

    internal class ExampleViewModel
    {
        public string Name { get; set; }
    }
}
