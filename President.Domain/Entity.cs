namespace President.Domain
{
    public abstract class Entity
    {


        protected void CheckRule(IBusinessRule rule)
        {
            rule.Check();
        }
    }
}
