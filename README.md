# Third-Person Shooter — IK and Partial Ragdoll Study

**A Unity character prototype exploring terrain-aware foot placement and the interaction between animation, limb damage, and physics.**

Built in 2021 with **Unity 2019.4.2f1**. The most useful part of this repository is its character-animation experiment: foot IK adapts to the ground, while damaged NPC limbs can follow physics-driven targets as the remaining animation continues.

Despite the repository name, this is a prototype with scene-specific setup, rather than a finished reusable shooter framework.

## Technical focus

### Terrain-aware foot placement

[FeetIKPlacementHandler](Assets/Scripts/FeetIKPlacementHandler.cs) samples beneath each humanoid foot using downward raycasts and a configurable terrain mask. Hit normals determine foot orientation. In `OnAnimatorIK`, the component applies Mecanim IK positions and rotations, smooths vertical foot offsets, and adjusts pelvis height using the lower foot's offset.

This is an application of Unity's humanoid IK solver; the repository does not implement a custom numerical IK solver.

### Physics-driven limb reactions

```text
Hit a body part
    → release its configured rigidbodies and IK target to physics
    → disable the corresponding animation layer
    → make the affected hand or foot follow that target through Animator IK
    → update hop / crawl state, or switch to full ragdoll on a fatal hit
```

The interesting integration is the handoff between animation layers, rigidbody motion, and IK targets. Individual limb reactions and full-body death are handled separately.

| Source | Responsibility |
| --- | --- |
| [FeetIKPlacementHandler](Assets/Scripts/FeetIKPlacementHandler.cs) | Ground sampling, foot alignment, and pelvis compensation |
| [HittableBodyPart](Assets/Scripts/HittableBodyPart.cs) | Route hits, release configured bodies, and apply impulses |
| [HittableBodyHandler](Assets/Scripts/HittableBodyHandler.cs) | Track affected limbs, alter animation state, and drive IK targets |
| [CharacterManipulatorScript](Assets/Scripts/CharacterManipulatorScript.cs) | Enable full-body ragdoll physics |
| [CharacterLocomotion](Assets/Scripts/CharacterLocomotion.cs) | Locomotion and chest rotation for aiming |
| [CharacterUserController](Assets/Scripts/CharacterUserController.cs) | Camera-relative movement and aiming input |

## Inspect the demo

1. Clone the repository and open its root with **Unity 2019.4.2f1**, matching [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt).
2. Start with [BodyPartsRagdoll.unity](Assets/Scenes/BodyPartsRagdoll.unity) for the limb experiment and [Level 1.unity](Assets/Scenes/Level%201.unity) for the shooter setup.
3. Check the humanoid Animator, IK-enabled layers, terrain mask, and serialized rigidbody/target references in the scene. These scripts depend on that configuration.

The input code uses the legacy `Horizontal` / `Vertical` axes, right mouse button for aiming, and left mouse button for firing. The committed build settings contain old scene references, so select the intended scenes explicitly when preparing a build.

The source and scene paths were reviewed for this documentation update. Visual stability, a fresh Unity import, and a standalone build have not been verified.

## Scope and tradeoffs

Foot IK uses full weights and a zero-vector sentinel for missing ground hits. It has no explicit planted/swing-foot weighting or moving-platform handling. Ground sampling occurs in `FixedUpdate`, while IK is applied in animation callbacks; that timing relationship needs evaluation in a polished controller.

The limb system relies on hard-coded animation-layer indices and configured references. A further iteration would validate those bindings, make damage states explicit, and test repeat hits and transitions between foot placement, injury animation, and full ragdoll.

These are useful animation/physics integration problems to discuss and demonstrate, with the limitations of the original prototype kept visible.

## Assets and attribution

The technical reading path is [Assets/Scripts](Assets/Scripts). The repository also bundles Unity Standard Assets/sample scenes, Nokobot handgun assets, QuickOutline, and supporting content. Those dependencies and assets are not presented as original work; their own notices and usage terms continue to apply.
