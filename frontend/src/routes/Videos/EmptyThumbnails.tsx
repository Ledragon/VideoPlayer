import { Box, Typography } from '@mui/material';

const EmptyThumbnails = () => {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', flexGrow: 1 }}>
      <img src={'/no-image-14600.svg'} alt="no image" />
      <Typography variant="body1">No thumbnail for this video</Typography>
    </Box>
  );
};

export default EmptyThumbnails;
