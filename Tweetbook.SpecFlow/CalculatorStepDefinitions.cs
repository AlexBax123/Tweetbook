using FluentAssertions;
using System;
using TechTalk.SpecFlow;

namespace Tweetbook.SpecFlow
{
    [Binding]
    public class CalculatorStepDefinitions
    {
        private int _p0;
        private int _p1;

        [Given(@"there are (.*) cucumbers")]
        public void GivenThereAreCucumbers(int start)
        {
            _p0 = start;
        }

        [When(@"I eat (.*) cucumbers")]
        public void WhenIEatCucumbers(int p0)
        {
            _p1 = p0;
        }

        [Then(@"I should have (.*) cucumbers")]
        public void ThenIShouldHaveCucumbers(int p0)
        {
            p0.Should().Be(_p0 - _p1);
        }
    }
}
