# Welcome

> Obviously there are no Don't Starve assets in this repo, that would be illegal.

>The assets that are in this repo are yours to use though.

## Camera setup:

A rotation of -45 feels good here, to zoom in and out change the y and z position values in lock step.

<img src="media/cameraSetup.png" width="500px"/>

## Sprite setup:

Sprites need to be rotated towards the camera, here I have a RotateOnAwake on every sprite in the scene.

<img src="media/rotate.png" width="500px"/>

For sorting to work make sure the game objects pivot point is at the bottom:

<img src="media/playerPivot.png" width="400px"><img src="media/treePivot.png" width="400px"/>

In your Renderer2D settings use the drop down to set Transparency Sort Mode to Custom Axis, then set the Transparency Sort Axis to (0, 0.5, 0.5)

<img src="media/renderer2D.png" width="500px"/>


