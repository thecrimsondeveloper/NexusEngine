# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

## [1.1.0] - 2024-08-06

### Added

- Implemented ISequence Events and added a changelog to track future updates.

## [1.0.9] - 2024-08-04

### Fixed

- Resolved build errors in `Sequence.cs`.

## [1.0.8] - 2024-08-03

### Fixed

- Fixed a data bug in `SequenceData` that was causing issues during sequence execution.
- Adjusted positioning logic.

### Removed

- Removed the UniTask version requirement to improve compatibility.

## [1.0.7] - 2024-08-02

### Updated

- Enhanced `CoreSequence.cs` to improve core sequence handling.
- Updated `Sequence.cs`, `MonoSequence.cs`, and `ScriptableSequence.cs` for better modularity and performance.

### Fixed

- Corrected settings instance and other sequence settings.
- Fixed issues related to Core Sequence.

## [1.0.6] - 2024-08-01

### Added

- Integrated a Sequence Generator tool (commits labeled as #7 and #6).
- Added `SequenceObject` for better sequence object management.

## [1.0.5] - 2024-07-31

### Added

- Implemented new events and transform handling in the sequence architecture.

### Updated

- Updated `package.json` multiple times to reflect dependency changes and other minor updates.

### Removed

- Removed old UniTask version requirements.

## [1.0.4] - 2024-07-30

### Updated

- Updated `SequenceController.cs` to support new sequence management features.

### Merged

- Merged pull request #4, which includes updates for data manipulation and sequence loading.

## [1.0.3] - 2024-07-29

### Fixed

- Fixed build errors and other minor issues with the `MonoSequence` prefab setup.

## [1.0.2] - 2024-07-28

### Updated

- Applied fixes and adjustments to ensure better performance and modularity across the NexusEngine package.

## [1.0.1] - 2024-07-27

### Added

- Initial release of the NexusEngine package with core features and initial setup.
