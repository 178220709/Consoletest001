using System;
using System.Collections;
using System.Threading;

public class Person
{
    public Person(string fName, string lName)
    {
        this.firstName = fName;
        this.lastName = lName;
    }

    public string firstName;
    public string lastName;
}

public class People : IEnumerable
{
    private Person[] _people;
    public People(Person[] pArray)
    {
        _people = new Person[pArray.Length];

        for (int i = 0; i < pArray.Length; i++)
        {
            _people[i] = pArray[i];
        }
    }

    public IEnumerator GetEnumerator()
    {
        return new PeopleEnum(_people);
    }

    public PeopleEnum GetEnumerator2()
    {
        return new PeopleEnum(_people);
    }
}

public class PeopleEnum : IEnumerator
{
    public Person[] _people;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    int position = -1;

    public PeopleEnum(Person[] list)
    {
        _people = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _people.Length);
    }

    public void Reset()
    {
        position = -1;
    }

    //object IEnumerator.Current
    //{
        
    //    get
    //    {
    //      //  throw new NotImplementedException();
    //        return Current;
    //    }
    //}

    public object Current
    {
        get
        {
            try
            {
                return _people[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

public class IEnumeratorTest
{
    public static void Main1()
    {
        var peopleArray = new Person[3]
        {
            new Person("John", "Smith"),
            new Person("Jim", "Johnson"),
            new Person("Sue", "Rabon"),
        };

        var peopleList = new People(peopleArray);
        foreach (MyCCC p in peopleList)
            Console.WriteLine(p.firstName + " " + p.lastName);

    }
}

public class MyCCC :IEnumerable
{
    public object firstName;
    public string lastName;

    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("before yield");
            yield return i;
            Thread .Sleep(3000);
            Console.WriteLine("after yield");
        }
    }
}


/* This code produces output similar to the following:
 *
 * John Smith
 * Jim Johnson
 * Sue Rabon
 *
 */