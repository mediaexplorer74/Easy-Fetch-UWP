// Decompiled with JetBrains decompiler
// Type: NiL.JS.CachedModuleResolverBase
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;

namespace NiL.JS
{
  public abstract class CachedModuleResolverBase : IModuleResolver
  {
    private StringMap<Module> _modulesCache = new StringMap<Module>();

    bool IModuleResolver.TryGetModule(ModuleRequest moduleRequest, out Module result)
    {
      string cacheKey = this.GetCacheKey(moduleRequest);
      if (this._modulesCache.TryGetValue(cacheKey, out result))
        return true;
      if (!this.TryGetModule(moduleRequest, out result))
        return false;
      this._modulesCache.Add(cacheKey, result);
      return true;
    }

    public abstract bool TryGetModule(ModuleRequest moduleRequest, out Module result);

    public virtual string GetCacheKey(ModuleRequest moduleRequest) => moduleRequest.AbsolutePath;

    public void RemoveFromCache(string key) => this._modulesCache.Remove(key);

    public void ClearCache() => this._modulesCache.Clear();
  }
}
