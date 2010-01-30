namespace Spss
{
	using System;
	using System.Diagnostics;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Collections.Generic;

	/// <summary>
	/// A collection of value labels for a <see cref="SpssVariable"/>.
	/// </summary>
	public abstract class SpssVariableValueLabelsDictionary<TKey> : IDictionary<TKey, string>
	{
		/// <summary>
		/// Tracks whether the value labels have been initialized from a data file yet.
		/// </summary>
		private bool isLoadedFromFileYet = false;

		private bool isLoading = false;

		private readonly Dictionary<TKey, string> ValuesLabels;
	
		private readonly SpssVariable variable;

		/// <summary>
		/// Creates an instance of the <see cref="SpssVariableValueLabelsDictionary&lt;TKey&gt;"/> class.
		/// </summary>
		/// <param name="variable">The hosting variable</param>
		/// <param name="comparer">The comparer; may be <c>null</c>.</param>
		protected SpssVariableValueLabelsDictionary(SpssVariable variable, IEqualityComparer<TKey> comparer)
		{
			if (variable == null) {
				throw new ArgumentNullException("variable");
			}

			this.variable = variable;
			ValuesLabels = new Dictionary<TKey, string>(4, comparer);
		}

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
		public string this [TKey Value]
		{
			get
			{
				LoadIfNeeded();
				return ValuesLabels[Value];
			}
			set
			{
				EnsureNotReadOnly();
				ValuesLabels[Value] = value;
			}
		}

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
		public virtual void Add(TKey value, string label)
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
		public bool Remove(TKey value)
		{
			EnsureNotReadOnly();
			return ValuesLabels.Remove(value);
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

		/// <summary>
		/// Copies the value labels defined in this list to another variable.
		/// </summary>
		public void CopyTo(SpssVariableValueLabelsDictionary<TKey> array)
		{
			if( array == null ) throw new ArgumentNullException("array");
			if( array.GetType() != GetType() )
				throw new ArgumentException("Copying value labels must be made to the same type of variable (not numeric to string or vice versa).", "array");
			foreach( var de in this )
				array.ValuesLabels.Add(de.Key, de.Value);
		}

		#endregion

		#region ICollection Members

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

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets the enumerator for the class.
		/// </summary>
		public IEnumerator<KeyValuePair<TKey, string>> GetEnumerator()
		{
			LoadIfNeeded();
			return ValuesLabels.GetEnumerator();
		}

		#endregion

		private void LoadIfNeeded() {
			// If we are working on a loaded file rather than a newly created
			// one, and if we have not yet loaded the value labels, load them now.
			if (IsReadOnly && !isLoadedFromFileYet && !Variable.CommittedThisSession) {
				Debug.Assert(ValuesLabels.Count == 0, "Somehow, a loaded file already has labels defined.");
				isLoading = true;
				LoadFromSpssFile();
				isLoading = false;
				isLoadedFromFileYet = true;
			}
		}

		#region IDictionary<TKey,string> Members

		public bool ContainsKey(TKey key) {
			return this.ValuesLabels.ContainsKey(key);
		}

		public ICollection<TKey> Keys {
			get { return this.ValuesLabels.Keys; }
		}

		public bool TryGetValue(TKey key, out string value) {
			return this.ValuesLabels.TryGetValue(key, out value);
		}

		public ICollection<string> Values {
			get { return this.ValuesLabels.Values; }
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,string>> Members

		public void Add(KeyValuePair<TKey, string> item) {
			EnsureNotReadOnly();
			this.ValuesLabels.Add(item.Key, item.Value);
		}

		public void Clear() {
			EnsureNotReadOnly();
			this.ValuesLabels.Clear();
		}

		public bool Contains(KeyValuePair<TKey, string> item) {
			return this.ContainsKey(item.Key);
		}

		public void CopyTo(KeyValuePair<TKey, string>[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<TKey, string> item) {
			return this.Remove(item.Key);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		#endregion
	}
}
