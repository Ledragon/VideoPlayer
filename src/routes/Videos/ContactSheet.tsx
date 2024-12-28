import { Box, Chip, Rating, Stack, Typography } from '@mui/material';

import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import { useGetContactSheetForVideoQuery, useGetThumbnailsForVideoQuery } from '../../redux';
import getBase64Url from './getBase64Url';
import ThumbsImageList from './ThumbsImageList';

interface PropType {
  id: number;
}

const ContactSheet = (props: PropType) => {
  const { id } = props;
  const { data: contactSheet, isLoading } = useGetContactSheetForVideoQuery(id);
  return (
    <Box
      sx={{
        flexGrow: 1,
        flexShrink: 0,
        display: 'flex'
      }}>
      {!contactSheet ? (
        <Box sx={{ display: 'flex', flexGrow: 1, justifyContent: 'center', alignItems: 'center' }}>
          <Typography>No contact sheet</Typography>
        </Box>
      ) : (
        <Box
          sx={{
            display: 'flex',
            flexGrow: 1,
            backgroundImage: contactSheet && `url('${getBase64Url(contactSheet.image)}')`,
            backgroundRepeat: 'no-repeat',
            backgroundSize: 'contain',
            backgroundPosition: 'center'
          }}></Box>
      )}

      {/* {contactSheet && <img src={getBase64Url(contactSheet.image)} style={{}} height="100%"></img>} */}
    </Box>
  );
};

export default ContactSheet;
