# Material UI to Mantine Migration Plan

## Project Overview

**Project:** NfoGenerator-ui  
**Current State:** React application built with Material UI v5  
**Target State:** Modern React application using Mantine UI library  
**Migration Strategy:** Theme-first, component-by-component gradual migration  

## Executive Summary

This document outlines a strategic plan to migrate the NfoGenerator-ui application from Material UI to Mantine. The approach prioritizes visual consistency and zero downtime by first implementing a Mantine-styled MUI theme, then gradually replacing components while maintaining full functionality.

## Current State Analysis

### Material UI Usage Assessment
- **40+ components** actively using MUI across the application
- **Core packages**: @mui/material, @mui/icons-material, @mui/lab, @mui/x-data-grid
- **Custom theming**: Dark/light mode support with custom color palette
- **Heavy sx prop usage**: Extensive styling with MUI's sx system
- **Complex components**: DataGrid, Autocomplete, Dialog, SpeedDial
- **Icons**: 15+ @mui/icons-material components

### Key Dependencies
```json
{
  "@mui/material": "5.10.16",
  "@mui/icons-material": "5.11.0", 
  "@mui/lab": "5.0.0-alpha.126",
  "@mui/x-data-grid": "6.18.1"
}
```

## Migration Strategy: Theme-First Approach

### Phase 1: Visual Transformation (Week 1)
**Goal**: Make the app look like Mantine while keeping MUI functionality

#### 1.1 Create Mantine-Styled MUI Theme
- **File**: `src/getMantineStyledTheme.ts`
- **Purpose**: Override MUI theme to adopt Mantine's design language
- **Benefits**: 
  - Immediate visual upgrade
  - Zero breaking changes
  - Preview of final Mantine look

#### 1.2 Theme Features
- **Colors**: Mantine blue primary palette with proper light/dark variants
- **Typography**: System fonts with Mantine sizing scale
- **Spacing**: Mantine's spacing system (xs: 10px, sm: 12px, md: 16px, lg: 20px, xl: 24px)
- **Shadows**: Subtle shadows matching Mantine aesthetic
- **Border Radius**: Mantine's radius scale (xs: 2px, sm: 4px, md: 8px, lg: 16px, xl: 32px)
- **Component Overrides**: 15+ MUI components styled to match Mantine

#### 1.3 Implementation
```tsx
// src/App.tsx
import getMantineStyledTheme from './getMantineStyledTheme';

const App = () => {
  return (
    <Provider store={store}>
      <ThemeProvider theme={getMantineStyledTheme('dark')}>
        <CssBaseline />
        <RouterProvider router={router} />
      </ThemeProvider>
    </Provider>
  );
};
```

### Phase 2: Setup Mantine Infrastructure (Week 1)
**Goal**: Prepare for gradual component migration

#### 2.1 Install Mantine Dependencies
```bash
npm install @mantine/core @mantine/hooks @mantine/dates @mantine/notifications @mantine/spotlight @tabler/icons-react
```

#### 2.2 Dual Provider Setup
```tsx
// Support both MUI and Mantine simultaneously
<Provider store={store}>
  <MantineProvider theme={mantineTheme}>
    <ThemeProvider theme={getMantineStyledTheme('dark')}>
      <CssBaseline />
      <RouterProvider router={router} />
    </ThemeProvider>
  </MantineProvider>
</Provider>
```

### Phase 3: Component Migration (Weeks 2-6)
**Goal**: Systematically replace MUI components with Mantine equivalents

#### 3.1 Migration Priority Matrix

| Priority | Component Category | Complexity | Timeline | Rationale |
|----------|-------------------|------------|----------|-----------|
| **High** | Simple UI Components | Low | Week 2 | Easy wins, immediate impact |
| **High** | Form Components | Medium | Week 3 | Core functionality, high usage |
| **Medium** | Layout Components | Medium | Week 4 | Structural, affects many pages |
| **Medium** | Navigation Components | Medium | Week 4 | User-facing, moderate complexity |
| **Low** | Complex Data Components | High | Week 5-6 | DataGrid consideration |
| **Low** | Specialized Components | High | Week 6 | Case-by-case evaluation |

#### 3.2 Detailed Migration Order

**Week 2: Simple Components**
1. `Typography` → `Text`
2. `Box` → `Group`/`Stack` 
3. `Button` → `Button`
4. `IconButton` → `ActionIcon`
5. `Chip` → `Chip`/`Badge`

**Week 3: Form Components** 
1. `TextField` → `TextInput`
2. `Autocomplete` → `MultiSelect`/`Select`
3. `Switch` → `Switch`
4. `Checkbox` → `Checkbox`
5. `Rating` → `Rating`

**Week 4: Layout & Navigation**
1. `Paper` → `Paper`
2. `Dialog` → `Modal`
3. `Tabs` → `Tabs`
4. `BottomNavigation` → Custom with Mantine components
5. `SpeedDial` → `Floating` or custom solution

**Week 5-6: Complex Components**
1. **DataGrid Decision Point**: Keep MUI X DataGrid vs migrate to alternative
2. `ImageList` → Custom grid with Mantine
3. Complex form dialogs
4. Video player integrations

### Phase 4: Icon Migration (Week 6)
**Goal**: Replace MUI icons with Tabler icons

#### 4.1 Icon Mapping Strategy
- Create icon mapping file: `src/utils/iconMapping.ts`
- Gradual replacement during component migration
- Maintain visual consistency

```tsx
// Example mapping
export const iconMap = {
  AccessTimeOutlined: IconClock,
  SortByAlphaOutlined: IconSortAscendingLetters,
  PlaylistAdd: IconPlaylistAdd,
  // ... continue mapping
};
```

### Phase 5: Cleanup & Optimization (Week 7)
**Goal**: Remove MUI dependencies and optimize bundle

#### 5.1 Dependency Cleanup
- Remove unused MUI packages
- Update package.json
- Remove MUI theme files
- Clean up imports

#### 5.2 Performance Optimization
- Bundle size analysis
- Tree shaking verification
- Performance testing

## Detailed Component Migration Examples

### Example 1: VideosFilters.tsx Migration

**Before (MUI):**
```tsx
import { Autocomplete, Box, FormControlLabel, IconButton, Paper, Switch, TextField } from '@mui/material';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import SortByAlphaOutlinedIcon from '@mui/icons-material/SortByAlphaOutlined';
```

**After (Mantine):**
```tsx
import { ActionIcon, Group, MultiSelect, Paper, Switch, TextInput } from '@mantine/core';
import { IconClock, IconSortAscendingLetters } from '@tabler/icons-react';
```

### Example 2: Complex Form Migration

**MUI Pattern:**
```tsx
<Dialog open={open}>
  <DialogTitle>Edit Video</DialogTitle>
  <DialogContent>
    <TextField fullWidth label="Title" />
    <Autocomplete options={tags} renderInput={(params) => <TextField {...params} />} />
  </DialogContent>
  <DialogActions>
    <Button onClick={onCancel}>Cancel</Button>
    <Button onClick={onSave}>Save</Button>
  </DialogActions>
</Dialog>
```

**Mantine Pattern:**
```tsx
<Modal opened={opened} onClose={onClose} title="Edit Video">
  <Stack>
    <TextInput label="Title" />
    <MultiSelect data={tags} label="Tags" />
    <Group justify="flex-end">
      <Button variant="outline" onClick={onCancel}>Cancel</Button>
      <Button onClick={onSave}>Save</Button>
    </Group>
  </Stack>
</Modal>
```

## Decision Points & Considerations

### DataGrid Strategy
**Options:**
1. **Keep MUI X DataGrid**: Maintain existing functionality, mixed UI
2. **Migrate to Mantine Table**: Simpler tables, lose advanced features  
3. **Use TanStack Table**: More flexibility, custom Mantine styling
4. **Hybrid Approach**: Keep DataGrid for complex tables, Mantine for simple ones

**Recommendation**: Keep MUI X DataGrid for now, evaluate alternatives in Phase 3

### Icon Strategy
**Current**: 15+ MUI icons  
**Target**: Tabler icons (Mantine's recommended icon library)  
**Approach**: Create mapping file, migrate incrementally

### Styling Strategy
**Current**: Heavy sx prop usage  
**Target**: Mantine's styling systems (style prop, CSS modules, emotion)  
**Transition**: Use style prop for simple styles, CSS modules for complex

## Risk Assessment & Mitigation

### High Risks
1. **DataGrid Functionality Loss**
   - *Mitigation*: Keep MUI DataGrid, evaluate alternatives separately
   
2. **Styling Inconsistencies During Transition**  
   - *Mitigation*: Phase 1 theme override ensures visual consistency

3. **Bundle Size Increase During Transition**
   - *Mitigation*: Monitor bundle size, clean up incrementally

### Medium Risks
1. **TypeScript Compatibility Issues**
   - *Mitigation*: Gradual migration allows fixing types incrementally
   
2. **Component Behavior Differences**
   - *Mitigation*: Thorough testing of each migrated component

### Low Risks
1. **Developer Learning Curve**
   - *Mitigation*: Mantine has excellent documentation and similar patterns

## Success Metrics

### Technical Metrics
- **Bundle Size**: Target 15-20% reduction after full migration
- **Performance**: Maintain or improve rendering performance
- **TypeScript Coverage**: 100% type safety maintained
- **Build Time**: No regression in build performance

### User Experience Metrics  
- **Visual Consistency**: 100% design system compliance
- **Functionality**: Zero feature regressions
- **Accessibility**: Maintain WCAG compliance
- **Mobile Responsiveness**: Improved mobile experience

## Timeline Summary

| Phase | Duration | Deliverables | 
|-------|----------|--------------|
| **Phase 1** | Week 1 | Mantine-styled MUI theme, visual transformation |
| **Phase 2** | Week 1 | Mantine setup, dual provider configuration |
| **Phase 3** | Weeks 2-6 | Component-by-component migration |
| **Phase 4** | Week 6 | Icon migration and mapping |
| **Phase 5** | Week 7 | Cleanup, optimization, documentation |

**Total Estimated Duration**: 7 weeks

## Migration Checklist

### Phase 1 Checklist
- [ ] Create `getMantineStyledTheme.ts`
- [ ] Update `App.tsx` with new theme
- [ ] Test visual consistency across all routes
- [ ] Verify dark/light mode functionality
- [ ] Document theme customizations

### Phase 2 Checklist  
- [ ] Install Mantine dependencies
- [ ] Setup dual provider configuration
- [ ] Create Mantine theme configuration
- [ ] Test both systems work together
- [ ] Update build configuration if needed

### Component Migration Checklist Template
For each component:
- [ ] Identify MUI components used
- [ ] Map to Mantine equivalents
- [ ] Update imports
- [ ] Convert props and styling
- [ ] Test functionality
- [ ] Test responsive behavior
- [ ] Update TypeScript types
- [ ] Code review and QA

### Final Cleanup Checklist
- [ ] Remove unused MUI dependencies
- [ ] Delete MUI theme files
- [ ] Update documentation
- [ ] Performance testing
- [ ] Bundle size analysis
- [ ] Accessibility audit
- [ ] Cross-browser testing

## Support Resources

### Documentation
- [Mantine Documentation](https://mantine.dev/)
- [Migration from MUI Guide](https://mantine.dev/guides/mui-migration/)
- [Tabler Icons Library](https://tabler-icons.io/)

### Team Resources
- Component migration examples in this document
- Code review guidelines for Mantine patterns
- Testing strategies for migrated components

---

**Document Version**: 1.0  
**Last Updated**: January 2025  
**Next Review**: End of Phase 1 