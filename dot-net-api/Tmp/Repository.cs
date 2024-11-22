namespace dot_net_api.Tmp;

public class Repository<T>
{
    private List<T> _data = new List<T>();

    private RepositoryOptions _repositoryOptions = new RepositoryOptions();

    public List<T> GetAll()
    {
        return _data;
    }

    public bool Add(T data)
    {
        if (_repositoryOptions.CheckPolicies(data))
        {
            _data.Add(data);
            return true;
        }

        return false;
    }


    public int Count(Predicate<T> predicate)
    {
        int counter = 0;
        foreach (var elem in _data)
        {
            if (predicate(elem))
            {
                counter++;
            }
        }

        return counter;
    }

    public void AddOptions(Action<RepositoryOptions> action)
    {
        action(_repositoryOptions);
    }
    

    public class RepositoryOptions
    {
        private Dictionary<string, Predicate<T>> _policies = new Dictionary<string, Predicate<T>>();
        
        public void AddPolicy(string policyName, Predicate<T> policyBuilder)
        {
            _policies.Add(policyName, policyBuilder);
        }

        public bool CheckPolicies(T element)
        {
            var succeededAllPolicies = true;
            foreach (var predicate in _policies.Values)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }

            return succeededAllPolicies;
        }
    }
    
}

