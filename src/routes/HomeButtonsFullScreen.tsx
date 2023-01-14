import { Box, Button, Stack } from '@mui/material';

const HomeButtonsFullScreen = (props: { buttons: any[] }) => {
  const { buttons } = props;
  return (
    <Box
      sx={{
        display: 'flex',
        flexGrow: 1,
        flexDirection: 'column'
      }}
    >
      <Box sx={{ flexBasis: '70%' }}></Box>
      <Stack
        alignItems="center"
        direction="row"
        justifyContent="center"
        sx={{
          flexGrow: 0,
          flexBasis: 'auto',
          backgroundColor: (theme) => theme.palette.grey[200],
          p: 2
        }}
        spacing={2}
      >
        {buttons.map((d, i) => (
          <Box key={i} sx={{ flexBasis: (1 / buttons.length) * 100 + '%', justifyContent: 'center', justifyItems: 'center', flexDirection: 'row', alignItems: 'center', display: 'flex' }}>
            <Button variant="contained">{d.label}</Button>
          </Box>
        ))}
      </Stack>
    </Box>
  );
};

export default HomeButtonsFullScreen;
