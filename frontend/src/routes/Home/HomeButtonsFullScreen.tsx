import { Box, Button } from '@mui/material';

const HomeButtonsFullScreen = (props: { buttons: any[] }) => {
  const { buttons } = props;
  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: 'auto auto',
        columnGap: 2,
        rowGap: 2,
        m: 2,
        flexGrow: 1
      }}>
      {buttons.map((d, i) => (
        <Box key={i} sx={{ display: 'inline-grid' }}>
          <Button variant="outlined">{d.label}</Button>
        </Box>
      ))}
    </Box>
  );
};

export default HomeButtonsFullScreen;
