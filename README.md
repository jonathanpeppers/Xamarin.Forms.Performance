# Xamarin.Forms.Performance

## Images

The idea for testing images is to just load 100 rows of 4 100x100 jpgs.

If you do this by normal means, the app completely falls over and you get a million "out of memory" errors in your console output.

The issue appears to be the use of `Bitmap` for resources... I would *never* do this in a non-Forms app, and instead use the native drawable/resource APIs.

Luckily, there is a means to make this happen without too much fuss.
When reviewing XF's source code it looks like there is some fallback logic checking if the `Bitmap` is null and using the resource APIs.

So we could:
1. Check if the image is a resource, and return `null` from the `FileImageSourceHandler`
2. Just let the resource fallback logic kick in
3. The app now works, I can scroll through my 400 images!

## Impact
If I can get a valid PR to Xamarin.Forms, this should speed up practically every XF image using the Android resource system. Hopefully most users are using this instead of the other options which will continue to be a bit flawed: Embedded Resource, download from web, plain file on disk, etc.

## TODO
To get a PR, I think I need to add some caching to `ResourceManager.GetDrawableByName` as it has heavy use of reflection. We will be calling it more often now. Should also make a new method that doesn't require the field to be read--just that it exists.

## TODO, TODO, One Day Maybe, We Can Dream
The API for `IImageSourceHandler` will hopelessly never be performant on Android due to the use of `Bitmap`.
Google has always recommended the use of `LRUCache` (least recently used cache) for caching/reusing `Bitmaps` throughout the life of an application.
Unfortunately, due to the nature of Xamarin.Forms there is not an easy way to see if a native view is actively *using* a `Bitmap`.
So that means we can't cache a copy of `IconIntheCorner.png` that happens to be on every `Page` of your application.
At some point the cache needs to get purged as you hit a memory threshold, if we happened to purge an image that is "least recently requested", but is still in use, we will crash.

In a past project I achieved something that solves this problem:
- Port the Java `LRUCache` to C#, so I could make changes
- Keep a `WeakReference` to any native view using a `Bitmap`, to keep the `LRUCache` from clearing `Bitmaps` in use
- Use my own `IImageSourceHandler` that requires the native view that is *using* the `Bitmap` to be passed in
- Make sure to call `Dipose()` and `Recycle()` appropriately on `Bitmaps` that get purged

This all worked great. Our app was image-heavy, and this implementation was the difference between the app working and it completely falling over.

Unfortunately, I don't know how this can be contributed back to Xamarin.Forms without API changes...