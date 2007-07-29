using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

namespace Spss
{
	/// <summary>
	/// A collection of value labels for a <see cref="SpssVariable"/>.
	/// </summary>
	public abstract class SpssVariableValueLabelsCollection : ICollection, IEnumerable
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssVariableValueLabelsCollection"/> class.
		/// </summary>
		/// <param name="variable">
		/// The hosting variable
		/// </param>
		protected SpssVariableValueLabelsCollection(SpssVariable variable)
		{
			if( variable == null ) throw new ArgumentNullException("variable");
			this.variable = variable;
			ValuesLabels = new HybridDictionary(4, false);
		}
		#endregion

		#region Attributes
		/// <summary>
		/// Tracks whether the value labels have been initialized from a data file yet.
		/// </summary>
		private bool isLoadedFromFileYet = false;

		/// <summary>
		/// Gets whether this list of value labels is read only.
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return Variable.Variables != null && Variable.Variables.IsReadOnly;
			}
		}
		private readonly HybridDictionary ValuesLabels;
		private readonly SpssVariable variable;
		/// <summary>
		/// The variable whose labels are being listed.
		/// </summary>
		protected SpssVariable Variable { get { return variable; } }
		/// <summary>
		/// Gets the SPSS file handle of the data document.
		/// </summary>
		protected int FileHandle
		{
			get
			{
				return Variable.Variables.Document.Handle;
			}
		}
		/// <summary>
		/// Gets/sets the response label for some response value.
		/// </summary>
		public string this [object Value]
		{
			get
			{
				LoadIfNeeded();
				return (string) ValuesLabels[Value];
			}
			set
			{
				EnsureNotReadOnly();
				ValuesLabels[Value] = value;
			}
		}
		#endregion

		#region Operations
		/// <summary>
		/// Adds a value label.
		/// </summary>
		/// <param name="value">
		/// The response value to associate with the new response label.
		/// </param>
		/// <param name="label">
		/// The new response label.
		/// </param>
		protected void Add(object value, string label)
		{
			EnsureNotReadOnly();
			ValuesLabels.Add(value, label);
		}
		/// <summary>
		/// Removes a value label.
		/// </summary>
		/// <param name="value">
		/// The response value to remove.
		/// </param>
		protected void Remove(object value)
		{
			EnsureNotReadOnly();
			ValuesLabels.Remove(value);
		}
		/// <summary>
		/// Updates the SPSS data file with changes made to the collection.
		/// </summary>
		protected internal abstract void Update();
		/// <summary>
		/// Initializes the value labels dictionary from the SPSS data file.
		/// </summary>
		protected abstract void LoadFromSpssFile();
		/// <summary>
		/// Throws an <see cref="InvalidOperationException"/> if the list of 
		/// value labels should not be altered at this state.
		/// </summary>
		protected void EnsureNotReadOnly()
		{
			if( IsReadOnly && !isLoading )
				throw new InvalidOperationException("Cannot perform this operation after dictionary has been committed.");
		}
		private bool isLoading = false;
		private void LoadIfNeeded()
		{
			// If we are working on a loaded file rather than a newly created
			// one, and if we have not yet loaded the value labels, load them now.
			if( IsReadOnly && !isLoadedFromFileYet && !Variable.CommittedThisSession)
			{
				Debug.Assert( ValuesLabels.Count == 0, "Somehow, a loaded file already has labels defined." );
				isLoading = true;
				LoadFromSpssFile();
				isLoading = false;
				isLoadedFromFileYet = true;
			}
		}
		/// <summary>
		/// Copies the value labels defined in this list to another variable.
		/// </summary>
		public void CopyTo(SpssVariableValueLabelsCollection array)
		{
			if( array == null ) throw new ArgumentNullException("array");
			if( array.GetType() != GetType() )
				throw new ArgumentException("Copying value labels must be made to the same type of variable (not numeric to string or vice versa).", "array");
			foreach( DictionaryEntry de in this )
				array.ValuesLabels.Add(de.Key, de.Value);
		}

		#endregion

		#region ICollection Members

		bool ICollection.IsSynchronized
		{
			get
			{
				return ValuesLabels.IsSynchronized;
			}
		}

		/// <summary>
		/// Gets the ICollection.IsSynchronized attribute.
		/// </summary>
		protected bool IsSynchronized { get { return ((ICollection)this).IsSynchronized; } }
		/// <summary>
		/// Gets the number of value labels that are defined.
		/// </summary>
		public int Count
		{
			get
			{
				LoadIfNeeded();
				return ValuesLabels.Count;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			ValuesLabels.CopyTo(array, index);
		}
		
		object ICollection.SyncRoot
		{
			get
			{
				return ValuesLabels.SyncRoot;
			}
		}
		/// <summary>
		/// Gets the ICollection.SyncRoot attribute.
		/// </summary>
		protected object SyncRoot { get { return ((ICollection)this).SyncRoot; } }

		#endregion

		#region IEnumerable Members
		/// <summary>
		/// Gets the enumerator for the class.
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			LoadIfNeeded();
			return ValuesLabels.GetEnumerator();
		}

		#endregion
	}
}
