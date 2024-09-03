namespace Game.Systems
{ 
    public enum ESystemAccessType
    {
        /// <summary>
        /// System cannot be accessed outside the creation scope
        /// </summary>
        Private,
        /// <summary>
        /// System can be accessed anywhere
        /// </summary>
        Public
    }
}