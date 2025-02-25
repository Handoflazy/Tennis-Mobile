using System.Collections.Generic;

public interface IObservableList<T> {
    /// <summary>
    /// Adds an item to the ObservableList.
    /// </summary>
    /// <param name="item">
    /// The item to add.
    /// </param>        
    void Add(T item);
    
    /// <summary>
    /// Inserts an item into the ObservableList at the specified index.
    /// </summary>
    /// <param name="index"> The zero-based index at which the item should be inserted. </param>
    /// <param name="item"> The item to insert into the ObservableList. </param>        
    void Insert(int index, T item);
    
    /// <summary>
    /// Removes all items from the ObservableList.
    /// </summary>        
    void Clear();
    
    /// <summary>
    /// Determines whether the ObservableList contains a specific item.
    /// </summary>
    /// <param name="item"> The item to locate. </param>        
    bool Contains(T item);
    
    /// <summary>
    /// Determines the index of a specific item in the IonObservableList.
    /// </summary>
    /// <param name="item"> The item to locate in the IonObservableList. </param>
    int IndexOf(T item);
    
    /// <summary>
    /// Copies the elements of the ObservableList to an array, starting at a particular index.
    /// </summary>
    /// <param name="array"> The one dimensional array that is the destination of the elements
    /// copied from the ObservableList. </param>
    /// <param name="arrayIndex"> The zero-based index in the array at which copying begins. </param>        
    void CopyTo(T[] array, int arrayIndex);
    
    /// <summary>
    /// Removes the first occurrence of a specific item from the ObservableList.
    /// </summary>
    /// <param name="item"> The item to remove from the ObservableList. </param>        
    bool Remove(T item);
    
    /// <summary>
    /// Returns a generic enumerator that iterates through the ObservableList.
    /// </summary>
    /// <returns>A generic enumerator that can be used to iterate through the collection.</returns>        
    IEnumerator<T> GetEnumerator();
    
    /// <summary>
    /// Removes the item at the specified index of the ObservableList.
    /// </summary>
    /// <param name="index"> The zero-based index of the item to remove. </param>        
    void RemoveAt(int index);
}