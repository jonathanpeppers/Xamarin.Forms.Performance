# Xamarin.Forms.Performance
Repo for testing performance in Xamarin.Forms on Android

Current Times from `Application.OnCreate` to `MainActivity.OnResume`:
| Device  | Configuration | Linker Setting | Time |
| ------- | ------- | -------- | ---------------- |
| Pixel 2 | Release | Link All | 00:00:00.6458096 |
|         |         |          | 00:00:00.7169119 |
|         |         |          | 00:00:00.6520137 |
| x86 emu | Release | Link All | 00:00:00.5080549 |
|         |         |          | 00:00:00.5016663 |
|         |         |          | 00:00:00.6002689 |