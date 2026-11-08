"""Encodes the rendered frames into an animated WebP that plays once and stops on its last frame.

Usage: python encode.py <framesDir> <out.webp> [frameMs] [quality]
"""
import glob
import sys

from PIL import Image

frames_dir, out = sys.argv[1], sys.argv[2]
frame_ms = int(sys.argv[3]) if len(sys.argv) > 3 else 40
quality = int(sys.argv[4]) if len(sys.argv) > 4 else 85

frames = [Image.open(p).convert("RGB") for p in sorted(glob.glob(f"{frames_dir}/f*.png"))]

frames[0].save(
    out,
    save_all=True,
    append_images=frames[1:],
    duration=frame_ms,
    loop=1,  # WebP counts plays: 0 repeats forever, 1 plays once.
    quality=quality,
    method=6,
    allow_mixed=True,
    minimize_size=True,
)
print(out, len(frames), "frames")
