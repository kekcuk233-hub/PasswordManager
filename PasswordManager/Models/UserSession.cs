public class UserSession : IDisposable
{
    private byte[]? _masterKey;
    private bool _disposed = false;

    public void SetKey(byte[] key)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(UserSession));
        _masterKey = key;
    }

    public byte[]? GetKey()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(UserSession));
        return _masterKey;
    }

    public bool IsLocked => _masterKey == null;

    public void Lock()
    {
        if (_masterKey != null)
        {
            Array.Clear(_masterKey, 0, _masterKey.Length);
            _masterKey = null;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Lock();
            _disposed = true;
        }
    }
}
