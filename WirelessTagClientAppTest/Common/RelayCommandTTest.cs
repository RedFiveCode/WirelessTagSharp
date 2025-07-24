using Xunit;
using System.Windows.Input;
using WirelessTagClientApp.Common;

namespace WirelessTagClientApp.Test.Common
{
    /// <summary>
    /// Unit tests for the <see cref="RelayCommandT"/> class
    /// </summary>
    public class RelayCommandTTest
    {
        [Fact]
        public void RelayCommandT_Implements_ICommand()
        {
            var target = new RelayCommandT<ExampleViewModel>(p => Execute(p));

            Assert.IsAssignableFrom<ICommand>(target);
        }

        [Fact]
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
            Assert.True(executeCalled);
        }

        [Fact]
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
            Assert.True(executeCalled);
            Assert.True(executeParameterIsNull);
        }

        [Fact]
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
            Assert.True(executeCalled);
            Assert.False(executeParameterIsNull);
        }

        [Fact]
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
            Assert.True(_canExecuteCalled);
        }

        [Fact]
        public void RelayCommandT_CanExecute_ReturnsExpectedValue()
        {
            // arrange
            var target = new RelayCommandT<ExampleViewModel>(p => Execute(p), p => CanExecute(p));
            var vm = new ExampleViewModel();

            // act
            var result = target.CanExecute(vm);

            // assert
            Assert.True(result);
        }

        [Fact]
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
            Assert.True(result);
            Assert.True(_canExecuteCalled);
            Assert.True(_canExecuteParameterIsNull);
        }

        [Fact]
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
            Assert.True(result);
            Assert.True(_canExecuteCalled);
            Assert.False(_canExecuteParameterIsNull);
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
