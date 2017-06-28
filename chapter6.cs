Chapter 6: I Don’t Have Much Time and I Have To Change It

I have started reading Working Effectively With Legacy Code as a way to start learning about proper unit testing and basically, as the title suggests, how to work with legacy code. The chapter I’m currently on is Chapter 6, which is the title of this post.

In this chapter, Feathers talks about how to introduce features/code into a class that isn’t readily testable. There are four ways to do so:

Sprout Method
Sprout Class
Wrap Method
Wrap Class
Examples

Here are some brief examples of each:

Sprout Method: Let’s say you want to introduce a feature to the following code -

public class TransactionGate
{
  public void postEntries(List entries) {
    for (Iterator it = entries.iterator(); it.hasNext();) {
      Entry entry = (Entry) it.next();
      entry.postDate();
    }
    transactionBundle.getListManager().add(entries);
  }
}
You want to check whether the entries are already in transactionBundle. You would normally just do that at the top right? Kinda…the sprout method way would be to move this logic to its own function and then call it. So this would be the final code.

public class TransactionGate
{
  public void postEntries(List entries) {
    List entriesToAdd = uniqueEntries(entries);
    for (Iterator it = entriesToAdd.iterator(); it.hasNext();) {
      Entry entry = (Entry) it.next();
      entry.postDate();
    }
    transactionBundle.getListManager().add(entriesToAdd);
  }
}
Sprout Class: Let’s say it’ll take too long to create a Sprout Method and separate things out; maybe the class is just too big. What do you do then? Well, you have to create a separate class to house the new functionality and then plug it in at the appropriate location. You might find, in the process of creating this class, that you can create an interface that both this new feature class and the existing class can inherit from and implement methods of. Thereby, making your existing code more testable.

Wrap Method: This one is easier to understand. First, you take the new method and turn it into its own function. Then you create a method that calls the new function AND the old one. So, it is a wrapper method around everything.
Wrapper Class: The book talks about this as an application of the decorator pattern. You can use the Extract Implementer to extend the class you’re adding the feature for and for the function you want to add the feature for, you simply override it, do your work, and then call the base implementation or vice versa. Instead of extending, you could also use the decorator pattern, where you have the new feature class take the existing class as a parameter, call its functionality, and then do your work in the applicable method.

class LoggingPayDispatcher {
  private Employee e;
  public LoggingPayDispatcher(Employee e) {
    this.e = e;
  }
  public void pay() {
    e.pay(); //Existing class implementation
    logPayment(); //New feature
  }
  private void logPayment() {
    // ...
  }
}
When To Use

So which one do you use when? Feathers makes the following recommendations:

I usually use Sprout Method when the code I have in the existing method communicates a clear algorithm to the reader. I move toward Wrap Method when I think that the new feature I’m adding is as important as the work that was there before.
…
Generally two cases tip me toward using Wrap Class :
The behavior that I want to add is completely independent, and I don’t want to pollute the existing class with behavior that is low level or unrelated.
The class has grown so large that I really can’t stand to make it worse, In a case like this, I wrap just to put a stake in the ground and provide a roadmap for later changes.