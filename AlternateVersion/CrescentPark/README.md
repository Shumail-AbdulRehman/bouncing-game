# Crescent Park — alternate Unity physics practical

This is a separate project for Unity **6000.6.1f1**. It uses a new scene layout and new C# source files. It includes an oval terracotta plaza, picnic tables, a decorative pond with stepping stones, faceted cedar trees, flower beds, garden lanterns, and a teal physics court. It replaces the earlier crossed-path park, spherical tree canopies, benches, and orange/blue ball pairing.

## Open and run

1. Extract the ZIP.
2. In Unity Hub, choose Add project from disk and select the `CrescentPark` folder (the folder containing Assets, Packages and ProjectSettings).
3. Open `Assets/Scenes/ParkPhysicsEnvironment.unity`.
4. Press Play. The coral and gold spheres fall and bounce on the court.
5. Click Release again to repeat, or Freeze to inspect a moment.

## Source code

- `Assets/Editor/CrescentParkFactory.cs`: generates all scene geometry, mesh assets, visual/physics materials, lighting and camera; validates physics; captures a screenshot.
- `Assets/Scripts/DropExperiment.cs`: records contacts and resets each sphere without adding bounce forces.
- `Assets/Scripts/ExperimentPanel.cs`: bottom toolbar with live height, impact counts, release and freeze controls.

## Assignment checklist

| Task | Implementation |
| --- | --- |
| Ground Plane, at least three Cubes, two Spheres, one Cylinder | Meadow Plane; cube picnic tables, planters and court; two dynamic spheres; cylinder trunks and plaza |
| Move, Rotate, Scale and materials | Positioned pond and furniture; angled table supports and entrance path; scaled primitives; distinct material assets |
| Two Rigidbody objects | Coral sphere: 0.75 kg; gold sphere: 2.5 kg; Use Gravity enabled; non-kinematic |
| Appropriate colliders | Ground Mesh Collider; cube Box Colliders; sphere Sphere Colliders; cylinder Capsule Colliders |
| Bounce | Coral bounciness 0.92; gold 0.35; Bounce Combine Maximum |
| Ground collision | Meadow Mesh Collider and court Box Collider; continuous dynamic collision detection on balls |
| Complete camera view | Orthographic overview camera |
| Meaningful hierarchy and required scene name | Named landscape, picnic garden, botanical border and physics court groups; ParkPhysicsEnvironment.unity |

The pond is decorative geometry, rather than fluid simulation. Cedar foliage is a visual mesh; cylinder trunks provide collision. Flat plaza and pond disks use static Mesh Colliders matching their shape; the dynamic ball experiments are on the separate flat Box Collider court.

## Submission and demonstration

Submit this project folder or the ZIP. `Evidence/CrescentPark.png` shows the completed environment. `Evidence/PhysicsValidation.txt` reports the tested bounce settings and 12-second simulation result.

To demonstrate component settings, select `Physics Court - Dynamic Spheres` → `Coral Sphere - Rubber` in the Hierarchy. Show its Rigidbody (Mass 0.75, Use Gravity enabled) and Sphere Collider. Open its assigned `Coral Sphere - Rubber Bounce` Physics Material in `Assets/Surfaces` to show Bounciness 0.92 and Bounce Combine Maximum. Repeat for the gold sphere if needed. Press Play and Release again to show the live bounce. A demonstration video is not included.

`Crescent Park → Check Physics` repeats validation outside Play mode. `Crescent Park → Generate New Park` replaces the supplied scene with a newly generated copy.

The archive intentionally excludes Unity-generated Library, Temp and Logs folders; Unity recreates these when opening the project. All source, scene, mesh/material assets, settings and screenshot are included.
